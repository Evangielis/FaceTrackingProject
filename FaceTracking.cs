using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace DF_FaceTracking.cs
{
    internal class FaceTracking
    {
        private readonly MainForm m_form;
        private FPSTimer m_timer;
        private bool m_wasConnected;

        public FaceTracking(MainForm form)
        {
            m_form = form;
        }

        private void DisplayDeviceConnection(bool isConnected)
        {
            if (isConnected && !m_wasConnected) m_form.UpdateStatus("Device Reconnected", MainForm.Label.StatusLabel);
            else if (!isConnected && m_wasConnected)
                m_form.UpdateStatus("Device Disconnected", MainForm.Label.StatusLabel);
            m_wasConnected = isConnected;
        }

        private void DisplayPicture(PXCMImage image)
        {
            PXCMImage.ImageData data;
            if (image.AcquireAccess(PXCMImage.Access.ACCESS_READ, PXCMImage.PixelFormat.PIXEL_FORMAT_RGB32, out data) <
                pxcmStatus.PXCM_STATUS_NO_ERROR) return;
            m_form.DrawBitmap(data.ToBitmap(0, image.info.width, image.info.height));
            m_timer.Tick("");
            image.ReleaseAccess(data);
        }

        private void CheckForDepthStream(PXCMCapture.Device.StreamProfileSet profiles, PXCMFaceModule faceModule)
        {
            PXCMFaceConfiguration faceConfiguration = faceModule.CreateActiveConfiguration();
            if (faceConfiguration == null)
            {
                Debug.Assert(faceConfiguration != null);
                return;
            }

            PXCMFaceConfiguration.TrackingModeType trackingMode = faceConfiguration.GetTrackingMode();
            faceConfiguration.Dispose();

            if (trackingMode != PXCMFaceConfiguration.TrackingModeType.FACE_MODE_COLOR_PLUS_DEPTH) return;
            if (profiles.depth.imageInfo.format == PXCMImage.PixelFormat.PIXEL_FORMAT_DEPTH) return;
            PXCMCapture.DeviceInfo dinfo;
            m_form.Devices.TryGetValue(m_form.GetCheckedDevice(), out dinfo);

            if (dinfo != null)
                MessageBox.Show(
                    String.Format("Depth stream is not supported for device: {0}. \nUsing 2D tracking", dinfo.name),
                    @"Face Tracking",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void FaceAlertHandler(PXCMFaceData.AlertData alert)
        {
            m_form.UpdateStatus(alert.label.ToString(), MainForm.Label.StatusLabel);
        }

        public void SimplePipeline()
        {
            PXCMSenseManager pp = m_form.Session.CreateSenseManager();

            if (pp == null)
            {
                throw new Exception("PXCMSenseManager null");
            }

            // Set Resolution
            var selectedRes = m_form.GetCheckedColorResolution();

            if (selectedRes != null && !m_form.GetPlaybackState())  
            {
                // activate filter only live/record mode , no need in playback mode
                var set = new PXCMCapture.Device.StreamProfileSet
                {
                    color =
                    {
                        frameRate = selectedRes.Item2,
                        imageInfo =
                        {
                            format = selectedRes.Item1.format,
                            height = selectedRes.Item1.height,
                            width = selectedRes.Item1.width
                        }
                    }
                };
                pp.captureManager.FilterByStreamProfiles(set);
            }

            // Set Source & Landmark Profile Index 
            PXCMCapture.DeviceInfo info;                        
            if (m_form.GetPlaybackState())
            {
                //pp.captureManager.FilterByStreamProfiles(null);
                pp.captureManager.SetFileName(m_form.GetFileName(), false);
                pp.captureManager.SetRealtime(false);
            }
            else  if (m_form.GetRecordState())
                {
                    pp.captureManager.SetFileName(m_form.GetFileName(), true);
                }
            
            // Set Module            
            pp.EnableFace();
            pp.EnableEmotion();
            PXCMFaceModule faceModule = pp.QueryFace();

            if (faceModule == null)
            {
                Debug.Assert(faceModule != null);
                return;
            }
            
            PXCMFaceConfiguration moduleConfiguration = faceModule.CreateActiveConfiguration();

            if (moduleConfiguration == null)
            {
                Debug.Assert(moduleConfiguration != null);
                return;
            }
            
            PXCMFaceConfiguration.TrackingModeType mode = m_form.GetCheckedProfile().Contains("3D")
                ? PXCMFaceConfiguration.TrackingModeType.FACE_MODE_COLOR_PLUS_DEPTH
                : PXCMFaceConfiguration.TrackingModeType.FACE_MODE_COLOR;

            moduleConfiguration.SetTrackingMode(mode);

            moduleConfiguration.strategy = PXCMFaceConfiguration.TrackingStrategyType.STRATEGY_RIGHT_TO_LEFT;

            moduleConfiguration.detection.maxTrackedFaces = m_form.NumDetection;
            moduleConfiguration.landmarks.maxTrackedFaces = m_form.NumLandmarks;
            moduleConfiguration.pose.maxTrackedFaces = m_form.NumPose;
            
            PXCMFaceConfiguration.ExpressionsConfiguration econfiguration = moduleConfiguration.QueryExpressions();
            if (econfiguration == null)
            {
                throw new Exception("ExpressionsConfiguration null");
            }
            econfiguration.properties.maxTrackedFaces = m_form.NumExpressions;

            econfiguration.EnableAllExpressions();
            moduleConfiguration.detection.isEnabled = m_form.IsDetectionEnabled();
            moduleConfiguration.landmarks.isEnabled = m_form.IsLandmarksEnabled();
            moduleConfiguration.pose.isEnabled = m_form.IsPoseEnabled();
            if (m_form.IsExpressionsEnabled())
            { 
                econfiguration.Enable();
            }

            PXCMFaceConfiguration.PulseConfiguration pulseConfiguration = moduleConfiguration.QueryPulse();
            if (pulseConfiguration == null)
            {
                throw new Exception("pulseConfiguration null");
            }
			
            pulseConfiguration.properties.maxTrackedFaces = m_form.NumPulse;
            if (m_form.IsPulseEnabled())
            {
                if (!m_form.GetPlaybackState())
                {
                    // Pulse requirement is 720p
                    pp.captureManager.FilterByStreamProfiles(null);
                    pp.captureManager.FilterByStreamProfiles(PXCMCapture.StreamType.STREAM_TYPE_COLOR, 1280, 720, 0);
                }
                pulseConfiguration.Enable();
            }

            PXCMFaceConfiguration.RecognitionConfiguration qrecognition = moduleConfiguration.QueryRecognition();
            if (qrecognition == null)
            {
                throw new Exception("PXCMFaceConfiguration.RecognitionConfiguration null");
            }
            if (m_form.IsRecognitionChecked())
            {
                qrecognition.Enable();
            }

            moduleConfiguration.EnableAllAlerts();
            moduleConfiguration.SubscribeAlert(FaceAlertHandler);

            pxcmStatus applyChangesStatus = moduleConfiguration.ApplyChanges();

            m_form.UpdateStatus("Init Started", MainForm.Label.StatusLabel);

            if (applyChangesStatus < pxcmStatus.PXCM_STATUS_NO_ERROR || pp.Init() < pxcmStatus.PXCM_STATUS_NO_ERROR)
            {
                m_form.UpdateStatus("Init Failed", MainForm.Label.StatusLabel);
            }
            else
            {
                using (PXCMFaceData moduleOutput = faceModule.CreateOutput())
                {
                    Debug.Assert(moduleOutput != null);
                    PXCMCapture.Device.StreamProfileSet profiles;

                    PXCMCaptureManager cmanager = pp.QueryCaptureManager();
                    if (cmanager == null)
                    {
                        throw new Exception("capture manager null");
                    }
                    PXCMCapture.Device device  =  cmanager.QueryDevice();

                    if (device == null)
                    {
                        throw new Exception("device null");
                    }
                    
                    device.QueryStreamProfileSet(PXCMCapture.StreamType.STREAM_TYPE_DEPTH, 0, out profiles);
                    CheckForDepthStream(profiles, faceModule);

                    m_form.UpdateStatus("Streaming", MainForm.Label.StatusLabel);
                    m_timer = new FPSTimer(m_form);

                    while (!m_form.Stopped)
                    {
                        //Query for emotion data
                        PXCMEmotion emoModule = pp.QueryEmotion();

                        if (pp.AcquireFrame(true) < pxcmStatus.PXCM_STATUS_NO_ERROR) break;
                        var isConnected = pp.IsConnected();
                        DisplayDeviceConnection(isConnected);
                        if (isConnected)
                        {
                            var sample = pp.QueryFaceSample();
                            if (sample == null)
                            {
                                pp.ReleaseFrame();
                                continue;
                            }
                            DisplayPicture(sample.color);

                            moduleOutput.Update();
                            if (moduleConfiguration.QueryRecognition().properties.isEnabled)
                                UpdateRecognition(moduleOutput);

                            m_form.DrawGraphics(moduleOutput);
                            m_form.UpdatePanel();
                            m_form.UpdateEmotion(emoModule);
                        }
                        pp.ReleaseFrame();
                    }
                }

                moduleConfiguration.UnsubscribeAlert(FaceAlertHandler);
                moduleConfiguration.ApplyChanges();
                m_form.UpdateStatus("Stopped", MainForm.Label.StatusLabel);
            }
            moduleConfiguration.Dispose();
            pp.Close();
            pp.Dispose();
        }

        private void UpdateRecognition(PXCMFaceData faceOutput)
        {
            //TODO: add null checks
            if (m_form.Register) RegisterUser(faceOutput);
            if (m_form.Unregister) UnregisterUser(faceOutput);
        }

        private void RegisterUser(PXCMFaceData faceOutput)
        {
            m_form.Register = false;
            if (faceOutput.QueryNumberOfDetectedFaces() <= 0)
                return;

            PXCMFaceData.Face qface = faceOutput.QueryFaceByIndex(0);
            if (qface == null)
            {
                throw new Exception("PXCMFaceData.Face null");
            }
            PXCMFaceData.RecognitionData rdata = qface.QueryRecognition();
            if (rdata == null)
            {
                throw new Exception(" PXCMFaceData.RecognitionData null");
            }
            rdata.RegisterUser();
        }

        private void UnregisterUser(PXCMFaceData faceOutput)
        {
            m_form.Unregister = false;
            if (faceOutput.QueryNumberOfDetectedFaces() <= 0)
            {
                return;
            }

            var qface = faceOutput.QueryFaceByIndex(0);
            if (qface == null)
            {
                throw new Exception("PXCMFaceData.Face null");
            }

            PXCMFaceData.RecognitionData rdata = qface.QueryRecognition();
            if (rdata == null)
            {
                throw new Exception(" PXCMFaceData.RecognitionData null");

            }

            if (!rdata.IsRegistered())
            {
                return;
            }
            rdata.UnregisterUser();
        }
    }
}