using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Threading;
using System.Windows.Forms;

namespace DF_FaceTracking.cs
{
    public partial class MainForm : Form
    {
        public enum Label
        {
            StatusLabel,
            AlertsLabel
        };

        

        private FaceTracking trackingModule;

        public PXCMSession Session;
        public volatile bool Register = false;
        public volatile bool Unregister = false;        
        public volatile bool Stopped = false;

        private readonly object m_bitmapLock = new object();
        private readonly FaceTextOrganizer m_faceTextOrganizer;
        private IEnumerable<CheckBox> m_modulesCheckBoxes;
        private IEnumerable<TextBox> m_modulesTextBoxes; 
        private Bitmap m_bitmap;
        private string m_filename;
        private Tuple<PXCMImage.ImageInfo, PXCMRangeF32> m_selectedColorResolution;
        private volatile bool m_closing;
        private static ToolStripMenuItem m_deviceMenuItem;
        private static ToolStripMenuItem m_moduleMenuItem;
        private static readonly int LANDMARK_ALIGNMENT = -3;

        public int PulseRate { get; private set; }
        public int NumFaces { get; private set; }
        public PXCMEmotion.EmotionData[] EmoData { get; private set; }
        public QuizMainForm qMain { get; private set; }
        public AnnoForm afMain { get; private set; }

        private string[] EmotionLabels = { "ANGER", "CONTEMPT", "DISGUST", "FEAR", "JOY", "SADNESS", "SURPRISE" };
        private string[] SentimentLabels = { "NEGATIVE", "POSITIVE", "NEUTRAL" };

        public int NUM_EMOTIONS = 10;
        public int NUM_PRIMARY_EMOTIONS = 7;

        public MainForm(PXCMSession session)
        {
            InitializeComponent();
            InitializeTextBoxes();

            this.qMain = new QuizMainForm(this);
            this.afMain = new AnnoForm();

            m_faceTextOrganizer = new FaceTextOrganizer();
            m_deviceMenuItem = new ToolStripMenuItem("Device");
            m_moduleMenuItem = new ToolStripMenuItem("Module");
            Session = session;
            CreateResolutionMap();
            PopulateDeviceMenu();
            PopulateModuleMenu();
            PopulateProfileMenu();
            InitializeCheckboxes();

            FormClosing += MainForm_FormClosing;
            Panel2.Paint += Panel_Paint;

            this.PulseRate = 0;
            this.NumFaces = 0;
        }

        private void InitializeTextBoxes()
        {
            m_modulesTextBoxes = new List<TextBox>
            {
                NumDetectionText,
                NumLandmarksText,
                NumPoseText,
                NumPulseText,
                NumExpressionsText,
            };

            foreach (var textBox in m_modulesTextBoxes)
            {
                textBox.Text = @"1";
            }
        }
        private void DisableUnsupportedAlgos()
        {
            Detection.Enabled = true;
            NumDetectionText.Enabled = true;
            bool enable = true;
            string deviceStr = GetCheckedDevice();
            if (deviceStr != null && deviceStr.Equals("Intel(R) DS4 Camera"))
            {
                enable = false;
                Expressions.Checked = false;
                Pulse.Checked = false;
                Recognition.Checked = false;
            }
            Landmarks.Enabled = enable;
            Landmarks.Checked = enable;
            NumLandmarksText.Enabled = enable;
            Pose.Enabled = enable;
            Pose.Checked = enable;
            NumPoseText.Enabled = enable;
            Expressions.Enabled = enable;
            NumExpressionsText.Enabled = enable;
            Recognition.Enabled = enable;
            Pulse.Enabled = enable;
            NumPulseText.Enabled = enable;
        }
        private void InitializeCheckboxes()
        {
            m_modulesCheckBoxes = new List<CheckBox>
            {
                Detection,
                Landmarks,
                Pose,
                Pulse,
                Expressions,
                Recognition
            };
            foreach (var checkBox in new []{ Detection, Pose, Landmarks, Expressions, Pulse})
            {
                checkBox.Checked = true;
                checkBox.Enabled = true;
            }
            DisableUnsupportedAlgos();
        }

        public Dictionary<string, PXCMCapture.DeviceInfo> Devices { get; set; }
        public Dictionary<string, IEnumerable<Tuple<PXCMImage.ImageInfo, PXCMRangeF32>>> ColorResolutions { get; set; }
        private readonly List<Tuple<int, int>> SupportedColorResolutions = new List<Tuple<int, int>>
        {
            Tuple.Create(1920, 1080),
            Tuple.Create(1280, 720),
            Tuple.Create(960, 540),
            Tuple.Create(640, 480),
            Tuple.Create(640, 360),
        };

        public int NumDetection
        {
            get 
            {
                int val;
                try
                {
                    val = Convert.ToInt32(NumDetectionText.Text); 
                }
                catch
                {
                    val = 0;
                }
                return val; 
            }
        }

        public int NumLandmarks
        {
            get 
            {
                int val;
                try
                {
                    val = Convert.ToInt32(NumLandmarksText.Text); 
                }
                catch
                {
                    val = 0;
                }
                return val; 
            }            
        }

        public int NumPose
        {
            get 
            {
                int val;
                try
                {
                    val = Convert.ToInt32(NumPoseText.Text); 
                }
                catch
                {
                    val = 0;
                }
                return val; 
            }             
        }

        public int NumExpressions
        {
            get 
            {
                int val;
                try
                {
                    val = Convert.ToInt32(NumExpressionsText.Text); 
                }
                catch
                {
                    val = 0;
                }
                return val; 
            }
        }

        public int NumPulse
        {
            get
            {
                int val;
                try
                {
                    val = Convert.ToInt32(NumPulseText.Text);
                }
                catch
                {
                    val = 0;
                }
                return val;
            }
        }

        public string GetFileName()
        {
            return m_filename;
        }

        public bool IsRecognitionChecked()
        {
            return Recognition.Checked;
        }

        private void CreateResolutionMap()
        {
            ColorResolutions = new Dictionary<string, IEnumerable<Tuple<PXCMImage.ImageInfo, PXCMRangeF32>>>();
            var desc = new PXCMSession.ImplDesc
            {
                group = PXCMSession.ImplGroup.IMPL_GROUP_SENSOR,
                subgroup = PXCMSession.ImplSubgroup.IMPL_SUBGROUP_VIDEO_CAPTURE
            };

            for (int i = 0; ; i++)
            {
                PXCMSession.ImplDesc desc1;
                if (Session.QueryImpl(desc, i, out desc1) < pxcmStatus.PXCM_STATUS_NO_ERROR) break;

                PXCMCapture capture;
                if (Session.CreateImpl(desc1, out capture) < pxcmStatus.PXCM_STATUS_NO_ERROR) continue;

                for (int j = 0; ; j++)
                {
                    PXCMCapture.DeviceInfo info;
                    if (capture.QueryDeviceInfo(j, out info) < pxcmStatus.PXCM_STATUS_NO_ERROR) break;

                    PXCMCapture.Device device = capture.CreateDevice(j);
                    if (device == null)
                    {
                        throw new Exception("PXCMCapture.Device null");
                    }
                    var deviceResolutions = new List<Tuple<PXCMImage.ImageInfo, PXCMRangeF32>>();

                    for (int k = 0; k < device.QueryStreamProfileSetNum(PXCMCapture.StreamType.STREAM_TYPE_COLOR); k++)
                    {
                        PXCMCapture.Device.StreamProfileSet profileSet;
                        device.QueryStreamProfileSet(PXCMCapture.StreamType.STREAM_TYPE_COLOR, k, out profileSet);
                        PXCMCapture.DeviceInfo dinfo;
                        device.QueryDeviceInfo(out dinfo);
                        var currentRes = new Tuple<PXCMImage.ImageInfo, PXCMRangeF32>(profileSet.color.imageInfo,
                            profileSet.color.frameRate);
                        if (profileSet.color.frameRate.min < 30 || (dinfo != null && dinfo.model == PXCMCapture.DeviceModel.DEVICE_MODEL_DS4 && profileSet.color.imageInfo.width == 1920))
                        {
                            continue;
                        }
                        if (SupportedColorResolutions.Contains(new Tuple<int, int>(currentRes.Item1.width, currentRes.Item1.height)))
                        {
                            deviceResolutions.Add(currentRes);
                        }
                    }
                    ColorResolutions.Add(info.name, deviceResolutions);
                    device.Dispose();
                }                              
                
                capture.Dispose();
            }
        }

        public void PopulateDeviceMenu()
        {
            Devices = new Dictionary<string, PXCMCapture.DeviceInfo>();
            var desc = new PXCMSession.ImplDesc
            {
                group = PXCMSession.ImplGroup.IMPL_GROUP_SENSOR,
                subgroup = PXCMSession.ImplSubgroup.IMPL_SUBGROUP_VIDEO_CAPTURE
            };
                        
            for (int i = 0; ; i++)
            {
                PXCMSession.ImplDesc desc1;
                if (Session.QueryImpl(desc, i, out desc1) < pxcmStatus.PXCM_STATUS_NO_ERROR) break;

                PXCMCapture capture;
                if (Session.CreateImpl(desc1, out capture) < pxcmStatus.PXCM_STATUS_NO_ERROR) continue;

                for (int j = 0; ; j++)
                {
                    PXCMCapture.DeviceInfo dinfo;
                    if (capture.QueryDeviceInfo(j, out dinfo) < pxcmStatus.PXCM_STATUS_NO_ERROR) break;
                    if (!Devices.ContainsKey(dinfo.name))
                        Devices.Add(dinfo.name, dinfo);
                    var sm1 = new ToolStripMenuItem(dinfo.name, null, Device_Item_Click);
                    m_deviceMenuItem.DropDownItems.Add(sm1);
                    sm1.Click += (sender, eventArgs) =>
                    {
                        DisableUnsupportedAlgos();
                    };
                }

                capture.Dispose();
            }

            if (m_deviceMenuItem.DropDownItems.Count > 0)
            {
                ((ToolStripMenuItem)m_deviceMenuItem.DropDownItems[0]).Checked = true;
                PopulateColorResolutionMenu(m_deviceMenuItem.DropDownItems[0].ToString());
            }

            try
            {
                MainMenu.Items.RemoveAt(0);
            }
            catch (NotSupportedException)
            {
                m_deviceMenuItem.Dispose();
                throw;
            }
            MainMenu.Items.Insert(0, m_deviceMenuItem);

            DisableUnsupportedAlgos();
        }

        public void PopulateColorResolutionMenu(string deviceName)
        {
            bool foundDefaultResolution = false;
            var sm = new ToolStripMenuItem("Color");
            foreach (var resolution in ColorResolutions[deviceName])
            {
                var resText = PixelFormat2String(resolution.Item1.format) + " " + resolution.Item1.width + "x"
                                 + resolution.Item1.height + " " + resolution.Item2.max + " fps";
                var sm1 = new ToolStripMenuItem(resText, null);
                var selectedResolution = resolution;
                sm1.Click += (sender, eventArgs) =>
                {
                    m_selectedColorResolution = selectedResolution;
                    ColorResolution_Item_Click(sender);
                };
            
                sm.DropDownItems.Add(sm1);

                if (selectedResolution.Item1.format == PXCMImage.PixelFormat.PIXEL_FORMAT_YUY2 && 
                    selectedResolution.Item1.width == 640 && selectedResolution.Item1.height == 360 && selectedResolution.Item2.min == 30)
                {
                    foundDefaultResolution = true;
                    sm1.Checked = true;
                    sm1.PerformClick();
                }
            }

	        if (!foundDefaultResolution && sm.DropDownItems.Count > 0)
	        {
	            ((ToolStripMenuItem)sm.DropDownItems[0]).Checked = true;
	            ((ToolStripMenuItem)sm.DropDownItems[0]).PerformClick();
	        }

            try
            {
                MainMenu.Items.RemoveAt(1);
            }
            catch (NotSupportedException)
            {
                sm.Dispose();
                throw;
            }
            MainMenu.Items.Insert(1, sm);
        }

        private void PopulateModuleMenu()
        {
            var desc = new PXCMSession.ImplDesc();
            desc.cuids[0] = PXCMFaceModule.CUID;
            
            for (int i = 0; ; i++)
            {
                PXCMSession.ImplDesc desc1;
                if (Session.QueryImpl(desc, i, out desc1) < pxcmStatus.PXCM_STATUS_NO_ERROR) break;
                var mm1 = new ToolStripMenuItem(desc1.friendlyName, null, Module_Item_Click);
                m_moduleMenuItem.DropDownItems.Add(mm1);
            }
            if (m_moduleMenuItem.DropDownItems.Count > 0)
                ((ToolStripMenuItem)m_moduleMenuItem.DropDownItems[0]).Checked = true;
            try
            {
                MainMenu.Items.RemoveAt(2);
            }
            catch (NotSupportedException)
            {
                m_moduleMenuItem.Dispose();
                throw;
            }
            MainMenu.Items.Insert(2, m_moduleMenuItem);
            
        }

        private void PopulateProfileMenu()
        {
            var pm = new ToolStripMenuItem("Profile");

            foreach (var trackingMode in (PXCMFaceConfiguration.TrackingModeType[])Enum.GetValues(typeof(PXCMFaceConfiguration.TrackingModeType)))
            {
                var pm1 = new ToolStripMenuItem(FaceMode2String(trackingMode), null, Profile_Item_Click);
                pm.DropDownItems.Add(pm1);

                if (trackingMode == PXCMFaceConfiguration.TrackingModeType.FACE_MODE_COLOR_PLUS_DEPTH) //3d = default
                {
                    pm1.Checked = true;
                }
            }
            try
            {
                MainMenu.Items.RemoveAt(3);
            }
            catch (NotSupportedException)
            {
                pm.Dispose();
                throw;
            }
            MainMenu.Items.Insert(3, pm);
        }

        private static string FaceMode2String(PXCMFaceConfiguration.TrackingModeType mode)
        {
            switch (mode)
            {
                case PXCMFaceConfiguration.TrackingModeType.FACE_MODE_COLOR:
                    return "2D Tracking";
                case PXCMFaceConfiguration.TrackingModeType.FACE_MODE_COLOR_PLUS_DEPTH:
                    return "3D Tracking";
            }
            return "";
        }

        private static string PixelFormat2String(PXCMImage.PixelFormat format)
        {
            switch (format)
            {
                case PXCMImage.PixelFormat.PIXEL_FORMAT_YUY2:
                    return "YUY2";
                case PXCMImage.PixelFormat.PIXEL_FORMAT_RGB32:
                    return "RGB32";
                case PXCMImage.PixelFormat.PIXEL_FORMAT_RGB24:
                    return "RGB24";                
            }
            return "NA";
        }

        private void RadioCheck(object sender, string name)
        {
            foreach (ToolStripMenuItem m in MainMenu.Items)
            {
                if (!m.Text.Equals(name)) continue;
                foreach (ToolStripMenuItem e1 in m.DropDownItems)
                {
                    e1.Checked = (sender == e1);
                }
            }
        }

        private void ColorResolution_Item_Click(object sender)
        {
            RadioCheck(sender, "Color");
        }

        private void Device_Item_Click(object sender, EventArgs e)
        {
            PopulateColorResolutionMenu(sender.ToString());
            RadioCheck(sender, "Device");
        }

        private void Module_Item_Click(object sender, EventArgs e)
        {
            RadioCheck(sender, "Module");
            PopulateProfileMenu();
        }

        private void Profile_Item_Click(object sender, EventArgs e)
        {
            RadioCheck(sender, "Profile");
        }

        private void Start_Click(object sender, EventArgs e)
        {
            Start.Enabled = false;
            MainMenu.Enabled = false;
            NumDetectionText.Enabled = false;
            NumLandmarksText.Enabled = false;
            NumPoseText.Enabled = false;
            NumExpressionsText.Enabled = false;
            NumPulseText.Enabled = false;
            Stop.Enabled = true;

            foreach (CheckBox moduleCheckBox in m_modulesCheckBoxes)
            {
                moduleCheckBox.Enabled = false;
            }

            if (Recognition.Checked)
            {
                RegisterUser.Enabled = true;
                UnregisterUser.Enabled = true;
            }

            this.trackingModule = new FaceTracking(this);

            Stopped = false;
            var thread = new Thread(DoTracking);
            thread.Start();

            if (!annoBox.Checked)
                qMain.ShowQuiz();
            else
                afMain.ShowMe();


            //Console.Out.WriteLine("Method completed!");
        }

        private void DoTracking()
        {
            var ft = this.trackingModule;
            
            ft.SimplePipeline();

            Invoke(new DoTrackingCompleted(() =>
            {
                foreach (CheckBox moduleCheckBox in m_modulesCheckBoxes)
                {
                    moduleCheckBox.Enabled = true;
                }
                DisableUnsupportedAlgos();
                Start.Enabled = true;
                Stop.Enabled = false;
                MainMenu.Enabled = true;

                //NumDetectionText.Enabled = true;
                //NumLandmarksText.Enabled = true;
                //NumPoseText.Enabled = true;
                //NumPulseText.Enabled = true;
                //NumExpressionsText.Enabled = true;

                RegisterUser.Enabled = false;
                UnregisterUser.Enabled = false;

                if (m_closing) Close();
            }));
        }

        public string GetCheckedDevice()
        {
            return (from ToolStripMenuItem m in MainMenu.Items
                where m.Text.Equals("Device")
                from ToolStripMenuItem e in m.DropDownItems
                where e.Checked
                select e.Text).FirstOrDefault();
        }

        public Tuple<PXCMImage.ImageInfo, PXCMRangeF32> GetCheckedColorResolution()
        {
            return m_selectedColorResolution;
        }

        public string GetCheckedModule()
        {
            return (from ToolStripMenuItem m in MainMenu.Items
                where m.Text.Equals("Module")
                from ToolStripMenuItem e in m.DropDownItems
                where e.Checked
                select e.Text).FirstOrDefault();
        }

        public string GetCheckedProfile()
        {
            foreach (var m in from ToolStripMenuItem m in MainMenu.Items where m.Text.Equals("Profile") select m)
            {
                for (var i = 0; i < m.DropDownItems.Count; i++)
                {
                    if (((ToolStripMenuItem) m.DropDownItems[i]).Checked)
                        return m.DropDownItems[i].Text;
                }
            }
            return "";
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Stopped = true;
            e.Cancel = Stop.Enabled;
            m_closing = true;
        }

        public void UpdateStatus(string status, Label label)
        {
            if (label == Label.StatusLabel)
                Status2.Invoke(new UpdateStatusDelegate(delegate(string s) { StatusLabel.Text = s; }),
                    new object[] {status});

            if (label == Label.AlertsLabel)
                Status2.Invoke(new UpdateStatusDelegate(delegate(string s) { AlertsLabel.Text = s; }),
                    new object[] {status});
        }

        public void StopMe() { Stopped = true; }

        private void Stop_Click(object sender, EventArgs e)
        {
            Stopped = true;
        }

        private void Panel_Paint(object sender, PaintEventArgs e)
        {
            lock (m_bitmapLock)
            {
                if (m_bitmap == null) return;
                if (Scale.Checked)
                {
                    e.Graphics.DrawImage(m_bitmap, Panel2.ClientRectangle);
                }
                else
                {
                    e.Graphics.DrawImageUnscaled(m_bitmap, 0, 0);
                }
            }
        }

        public void UpdatePanel()
        {
            Panel2.Invoke(new UpdatePanelDelegate(() => Panel2.Invalidate()));
        }

        public void DrawBitmap(Bitmap picture)
        {
            lock (m_bitmapLock)
            {
                if (m_bitmap != null)
                {
                    m_bitmap.Dispose();
                }
                m_bitmap = new Bitmap(picture);
            }
        }

        public void DrawGraphics(PXCMFaceData moduleOutput)
        {
            Debug.Assert(moduleOutput != null);

            this.NumFaces = moduleOutput.QueryNumberOfDetectedFaces();

            for (var i = 0; i < this.NumFaces; i++)
            {
                PXCMFaceData.Face face = moduleOutput.QueryFaceByIndex(i);
                if (face == null)
                {
                    throw new Exception("DrawGraphics::PXCMFaceData.Face null");
                }
                
                lock (m_bitmapLock)
                {
                    m_faceTextOrganizer.ChangeFace(i, face, m_bitmap.Height, m_bitmap.Width);
                }

                DrawLocation(face);
                DrawLandmark(face);
                DrawPose(face);
                DrawPulse(face);
                DrawExpressions(face);
                DrawRecognition(face);
            }

            DrawQuizInfo();
        }

        private void RegisterUser_Click(object sender, EventArgs e)
        {
            Register = true;
        }

        private void UnregisterUser_Click(object sender, EventArgs e)
        {
            Unregister = true;
        }

        #region Playback / Record

        private void Live_Click(object sender, EventArgs e)
        {
            Playback.Checked = Record.Checked = false;
            Live.Checked = true;
        }

        private void Playback_Click(object sender, EventArgs e)
        {
            Live.Checked = Record.Checked = false;
            Playback.Checked = true;
            var ofd = new OpenFileDialog
            {
                Filter = @"RSSDK clip|*.rssdk|Old format clip|*.pcsdk|All files|*.*",
                CheckFileExists = true,
                CheckPathExists = true
            };
            try
            {
                m_filename = (ofd.ShowDialog() == DialogResult.OK) ? ofd.FileName : null;                
            }
            catch (Exception)
            {
                ofd.Dispose();
                throw;
            }
            ofd.Dispose();
        }

        public bool GetPlaybackState()
        {
            return Playback.Checked;
        }

        private void Record_Click(object sender, EventArgs e)
        {
            Live.Checked = Playback.Checked = false;
            Record.Checked = true;
            var sfd = new SaveFileDialog
            {
                Filter = @"RSSDK clip|*.rssdk|All files|*.*",
                CheckPathExists = true,
                OverwritePrompt = true,
                AddExtension    = true
            };
            try
            {
                m_filename = (sfd.ShowDialog() == DialogResult.OK) ? sfd.FileName : null;
            }
            catch (Exception)
            {
                sfd.Dispose();
                throw;
            }
            sfd.Dispose();
        }

        public bool GetRecordState()
        {
            return Record.Checked;
        }

        public string GetPlaybackFile()
        {
            return Invoke(new GetFileDelegate(() =>
            {
                var ofd = new OpenFileDialog
                {
                    Filter = @"All files (*.*)|*.*",
                    CheckFileExists = true,
                    CheckPathExists = true
                };
                return ofd.ShowDialog() == DialogResult.OK ? ofd.FileName : null;
            })) as string;
        }

        public string GetRecordFile()
        {
            return Invoke(new GetFileDelegate(() =>
            {
                var sfd = new SaveFileDialog
                {
                    Filter = @"All files (*.*)|*.*",
                    CheckFileExists = true,
                    OverwritePrompt = true
                };
                if (sfd.ShowDialog() == DialogResult.OK) return sfd.FileName;
                return null;
            })) as string;
        }

        private delegate string GetFileDelegate();

        #endregion

        #region Modules Drawing

        private static readonly Assembly m_assembly = Assembly.GetExecutingAssembly();

        private readonly ResourceSet m_resources = 
            new ResourceSet(m_assembly.GetManifestResourceStream(@"DF_FaceTracking.cs.Properties.Resources.resources"));

        private readonly Dictionary<PXCMFaceData.ExpressionsData.FaceExpression, Bitmap> m_cachedExpressions =
            new Dictionary<PXCMFaceData.ExpressionsData.FaceExpression, Bitmap>();

        private readonly Dictionary<PXCMFaceData.ExpressionsData.FaceExpression, string> m_expressionDictionary =
            new Dictionary<PXCMFaceData.ExpressionsData.FaceExpression, string>
            {
                {PXCMFaceData.ExpressionsData.FaceExpression.EXPRESSION_MOUTH_OPEN, @"MouthOpen"},
                {PXCMFaceData.ExpressionsData.FaceExpression.EXPRESSION_SMILE, @"Smile"},
                {PXCMFaceData.ExpressionsData.FaceExpression.EXPRESSION_KISS, @"Kiss"},
                {PXCMFaceData.ExpressionsData.FaceExpression.EXPRESSION_EYES_UP, @"Eyes_Turn_Up"},
                {PXCMFaceData.ExpressionsData.FaceExpression.EXPRESSION_EYES_DOWN, @"Eyes_Turn_Down"},
                {PXCMFaceData.ExpressionsData.FaceExpression.EXPRESSION_EYES_TURN_LEFT, @"Eyes_Turn_Left"},
                {PXCMFaceData.ExpressionsData.FaceExpression.EXPRESSION_EYES_TURN_RIGHT, @"Eyes_Turn_Right"},
                {PXCMFaceData.ExpressionsData.FaceExpression.EXPRESSION_EYES_CLOSED_LEFT, @"Eyes_Closed_Left"},
                {PXCMFaceData.ExpressionsData.FaceExpression.EXPRESSION_EYES_CLOSED_RIGHT, @"Eyes_Closed_Right"},
                {PXCMFaceData.ExpressionsData.FaceExpression.EXPRESSION_BROW_LOWERER_RIGHT, @"Brow_Lowerer_Right"},
                {PXCMFaceData.ExpressionsData.FaceExpression.EXPRESSION_BROW_LOWERER_LEFT, @"Brow_Lowerer_Left"},
                {PXCMFaceData.ExpressionsData.FaceExpression.EXPRESSION_BROW_RAISER_RIGHT, @"Brow_Raiser_Right"},
                {PXCMFaceData.ExpressionsData.FaceExpression.EXPRESSION_BROW_RAISER_LEFT, @"Brow_Raiser_Left"}
            };

        public Dictionary<PXCMFaceData.ExpressionsData.FaceExpression, int> m_expressionStatus =
            new Dictionary<PXCMFaceData.ExpressionsData.FaceExpression, int>
            {
                {PXCMFaceData.ExpressionsData.FaceExpression.EXPRESSION_MOUTH_OPEN, 0},
                {PXCMFaceData.ExpressionsData.FaceExpression.EXPRESSION_SMILE, 0},
                {PXCMFaceData.ExpressionsData.FaceExpression.EXPRESSION_KISS, 0},
                {PXCMFaceData.ExpressionsData.FaceExpression.EXPRESSION_EYES_UP, 0},
                {PXCMFaceData.ExpressionsData.FaceExpression.EXPRESSION_EYES_DOWN, 0},
                {PXCMFaceData.ExpressionsData.FaceExpression.EXPRESSION_EYES_TURN_LEFT, 0},
                {PXCMFaceData.ExpressionsData.FaceExpression.EXPRESSION_EYES_TURN_RIGHT, 0},
                {PXCMFaceData.ExpressionsData.FaceExpression.EXPRESSION_EYES_CLOSED_LEFT, 0},
                {PXCMFaceData.ExpressionsData.FaceExpression.EXPRESSION_EYES_CLOSED_RIGHT, 0},
                {PXCMFaceData.ExpressionsData.FaceExpression.EXPRESSION_BROW_LOWERER_RIGHT, 0},
                {PXCMFaceData.ExpressionsData.FaceExpression.EXPRESSION_BROW_LOWERER_LEFT, 0},
                {PXCMFaceData.ExpressionsData.FaceExpression.EXPRESSION_BROW_RAISER_RIGHT, 0},
                {PXCMFaceData.ExpressionsData.FaceExpression.EXPRESSION_BROW_RAISER_LEFT, 0}
            };

        public void DrawQuizInfo()
        {
            if (m_bitmap == null || this.qMain == null)
                return;
                
            string timestamp = (annoBox.Checked) ? afMain.Clock.ElapsedMilliseconds.ToString() : qMain.Clock.ElapsedMilliseconds.ToString();

            lock(m_bitmapLock)
            {
                using (Graphics graphics = Graphics.FromImage(m_bitmap))
                using (var pen = new Pen(m_faceTextOrganizer.Colour, 3.0f))
                using (var brush = new SolidBrush(Color.Red))
                using (var font = new Font(FontFamily.GenericMonospace, m_faceTextOrganizer.FontSize * 2, FontStyle.Bold))                
                {
                    if ((this.qMain.qEnum != null) && (this.qMain.qEnum.QIndex < this.qMain.qEnum.Count))
                    {
                        timestamp += "  #" + (this.qMain.qEnum.QIndex + 1).ToString();
                    }

                    SizeF tsSize = graphics.MeasureString(timestamp, font);
                    graphics.FillRectangle(new SolidBrush(Color.Black), new Rectangle(0, 0, Convert.ToInt32(tsSize.Width + 20), Convert.ToInt32(tsSize.Height + 10)));
                    graphics.DrawString(timestamp, font, brush, m_faceTextOrganizer.TimeStampLoc);
                }
            }
        }

        public void DrawLocation(PXCMFaceData.Face face)
        {
            Debug.Assert(face != null);
            if (m_bitmap == null || !Detection.Checked) return;

            PXCMFaceData.DetectionData detection = face.QueryDetection();
            if (detection == null)
                return;

            lock (m_bitmapLock)
            {
                using (Graphics graphics = Graphics.FromImage(m_bitmap))
                using (var pen = new Pen(m_faceTextOrganizer.Colour, 3.0f))
                using (var brush = new SolidBrush(m_faceTextOrganizer.Colour))
                using (var font = new Font(FontFamily.GenericMonospace, m_faceTextOrganizer.FontSize, FontStyle.Bold))
                {
                    graphics.DrawRectangle(pen, m_faceTextOrganizer.RectangleLocation);
                    String faceId = String.Format("Face ID: {0}",
                        face.QueryUserID().ToString(CultureInfo.InvariantCulture));
                    graphics.DrawString(faceId, font, brush, m_faceTextOrganizer.FaceIdLocation);
                }
            }
        }
            
        public void DrawLandmark(PXCMFaceData.Face face)
        {
            Debug.Assert(face != null);
            PXCMFaceData.LandmarksData landmarks = face.QueryLandmarks();
            if (m_bitmap == null || !Landmarks.Checked || landmarks == null) return;

            lock (m_bitmapLock)
            {
                using (Graphics graphics = Graphics.FromImage(m_bitmap))
                using (var brush = new SolidBrush(Color.White))
                using (var lowConfidenceBrush = new SolidBrush(Color.Red))
                using (var font = new Font(FontFamily.GenericMonospace, m_faceTextOrganizer.FontSize, FontStyle.Bold))
                {
                    PXCMFaceData.LandmarkPoint[] points;
                    bool res = landmarks.QueryPoints(out points);
                    Debug.Assert(res);

                    var point = new PointF();

                    foreach (PXCMFaceData.LandmarkPoint landmark in points)
                    {
                        point.X = landmark.image.x + LANDMARK_ALIGNMENT;
                        point.Y = landmark.image.y + LANDMARK_ALIGNMENT;

                        if (landmark.confidenceImage == 0)
                            graphics.DrawString("x", font, lowConfidenceBrush, point);
                        else
                            graphics.DrawString("•", font, brush, point);
                    }
                }
            }
        }

        public void DrawPose(PXCMFaceData.Face face)
        {
            Debug.Assert(face != null);
            PXCMFaceData.PoseEulerAngles poseAngles;
            PXCMFaceData.PoseData pdata = face.QueryPose();
            if (pdata == null)
            {
                return;
            }
            if (!Pose.Checked || !pdata.QueryPoseAngles(out poseAngles)) return;

            lock (m_bitmapLock)
            {
                using (Graphics graphics = Graphics.FromImage(m_bitmap))
                using (var brush = new SolidBrush(m_faceTextOrganizer.Colour))
                using (var font = new Font(FontFamily.GenericMonospace, m_faceTextOrganizer.FontSize, FontStyle.Bold))
                {
                    string yawText = String.Format("Yaw = {0}",
                        Convert.ToInt32(poseAngles.yaw).ToString(CultureInfo.InvariantCulture));
                    graphics.DrawString(yawText, font, brush, m_faceTextOrganizer.PoseLocation.X,
                        m_faceTextOrganizer.PoseLocation.Y);

                    string pitchText = String.Format("Pitch = {0}",
                        Convert.ToInt32(poseAngles.pitch).ToString(CultureInfo.InvariantCulture));
                    graphics.DrawString(pitchText, font, brush, m_faceTextOrganizer.PoseLocation.X,
                        m_faceTextOrganizer.PoseLocation.Y + m_faceTextOrganizer.FontSize);

                    string rollText = String.Format("Roll = {0}",
                        Convert.ToInt32(poseAngles.roll).ToString(CultureInfo.InvariantCulture));
                    graphics.DrawString(rollText, font, brush, m_faceTextOrganizer.PoseLocation.X,
                        m_faceTextOrganizer.PoseLocation.Y + 2 * m_faceTextOrganizer.FontSize);
                }
            }
        }

        public void DrawExpressions(PXCMFaceData.Face face)
        {
            Debug.Assert(face != null);
            if (m_bitmap == null || !Expressions.Checked) return;

            PXCMFaceData.ExpressionsData expressionsOutput = face.QueryExpressions();

            if (expressionsOutput == null) return;

            lock (m_bitmapLock)
            {
                using (Graphics graphics = Graphics.FromImage(m_bitmap))
                using (var brush = new SolidBrush(m_faceTextOrganizer.Colour))
                {
                    const int imageSizeWidth = 18;
                    const int imageSizeHeight = 18;

                    int positionX = m_faceTextOrganizer.ExpressionsLocation.X;
                    int positionXText = positionX + imageSizeWidth;
                    int positionY = m_faceTextOrganizer.ExpressionsLocation.Y;
                    int positionYText = positionY + imageSizeHeight / 4;

                    foreach (var expressionEntry in m_expressionDictionary)
                    {
                        PXCMFaceData.ExpressionsData.FaceExpression expression = expressionEntry.Key;
                        PXCMFaceData.ExpressionsData.FaceExpressionResult result;
                        bool status = expressionsOutput.QueryExpression(expression, out result);
                        if (!status) continue;

                        Bitmap cachedExpressionBitmap;
                        bool hasCachedExpressionBitmap = m_cachedExpressions.TryGetValue(expression, out cachedExpressionBitmap);
                        if (!hasCachedExpressionBitmap)
                        {
                            cachedExpressionBitmap = (Bitmap) m_resources.GetObject(expressionEntry.Value);
                            m_cachedExpressions.Add(expression, cachedExpressionBitmap);
                        }

                        using (var font = new Font(FontFamily.GenericMonospace, m_faceTextOrganizer.FontSize, FontStyle.Bold))
                        {
                            Debug.Assert(cachedExpressionBitmap != null, "cachedExpressionBitmap != null");
                            graphics.DrawImage(cachedExpressionBitmap, new Rectangle(positionX, positionY, imageSizeWidth, imageSizeHeight));
                            var expressionText = String.Format("= {0}", result.intensity);
                            graphics.DrawString(expressionText, font, brush, positionXText, positionYText);

                            positionY += imageSizeHeight;
                            positionYText += imageSizeHeight;
                        }

                        //Save expression data
                        this.m_expressionStatus[expressionEntry.Key] = result.intensity;
                    }
                }
            }
        }

        public void DrawRecognition(PXCMFaceData.Face face)
        {
            Debug.Assert(face != null);
            if (m_bitmap == null || !Recognition.Checked) return;

            PXCMFaceData.RecognitionData qrecognition = face.QueryRecognition();
            if (qrecognition == null)
            {
                throw new Exception(" PXCMFaceData.RecognitionData null");
            }
            var userId = qrecognition.QueryUserID();
            var recognitionText = userId == -1 ? "Not Registered" : String.Format("Registered ID: {0}", userId);

            lock (m_bitmapLock)
            {
                using (Graphics graphics = Graphics.FromImage(m_bitmap))
                using (var brush = new SolidBrush(m_faceTextOrganizer.Colour))
                using (var font = new Font(FontFamily.GenericMonospace, m_faceTextOrganizer.FontSize, FontStyle.Bold))
                {
                    graphics.DrawString(recognitionText, font, brush, m_faceTextOrganizer.RecognitionLocation);
                }
            }
        }
        
        private void DrawPulse(PXCMFaceData.Face face)
        {
            Debug.Assert(face != null);
            if (m_bitmap == null || !Pulse.Checked) return;

            var pulseData = face.QueryPulse();
            if (pulseData == null)            
                return;

            float prate = pulseData.QueryHeartRate();
            
            //Save Pulse
            this.PulseRate = (int)prate;

            lock (m_bitmapLock)
            {
                var pulseString = "Pulse: " + prate;
                
                using (var graphics = Graphics.FromImage(m_bitmap))
                using (var brush = new SolidBrush(m_faceTextOrganizer.Colour))
                using (var font = new Font(FontFamily.GenericMonospace, m_faceTextOrganizer.FontSize, FontStyle.Bold))
                {
                    graphics.DrawString(pulseString, font, brush, m_faceTextOrganizer.PulseLocation);
                }
            }
        }

        #endregion

        private delegate void DoTrackingCompleted();

        private delegate void UpdatePanelDelegate();

        private delegate void UpdateStatusDelegate(string status);

        private void Detection_CheckedChanged(object sender, EventArgs e)
        {
            NumDetectionText.Enabled = Detection.Checked;
        }

        private void Landmarks_CheckedChanged(object sender, EventArgs e)
        {
            NumLandmarksText.Enabled = Landmarks.Checked;
        }

        private void Pose_CheckedChanged(object sender, EventArgs e)
        {
            NumPoseText.Enabled = Pose.Checked;
        }

        private void Expressions_CheckedChanged(object sender, EventArgs e)
        {
            NumExpressionsText.Enabled = Expressions.Checked;
        }

        private void Pulse_CheckedChanged(object sender, EventArgs e)
        {
            NumPulseText.Enabled = Pulse.Checked;
        }

        public bool IsDetectionEnabled()
        {
            return Detection.Checked;
        }

        public bool IsLandmarksEnabled()
        {
            return Landmarks.Checked;
        }

        public bool IsPoseEnabled()
        {
            return Pose.Checked;
        }

        public bool IsExpressionsEnabled()
        {
            return Expressions.Checked;
        }

        public bool IsPulseEnabled()
        {
            return Pulse.Checked;
        }

        private void NumDetectionText_TextChanged(object sender, EventArgs e)
        {

        }

        private void questionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            QuizEditor editor = new QuizEditor();
            editor.Show();
        }

        private void Panel2_Click(object sender, EventArgs e)
        {

        }

        public void UpdateEmotion(PXCMEmotion emoMod)
        {
            if (emoMod.QueryNumFaces() == 0)
                return;

            /* Retrieve emotionDet location data */
            PXCMEmotion.EmotionData[] arrData = new PXCMEmotion.EmotionData[this.NUM_EMOTIONS];
            if (emoMod.QueryAllEmotionData(0, out arrData) >= pxcmStatus.PXCM_STATUS_NO_ERROR)
            {
                this.EmoData = arrData;
            }
        }
    }
}
