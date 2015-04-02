using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace emotion_viewer.cs
{
    
    class EmotionDetection
    {

        private MainForm form;
        private bool disconnected = false;
        private FPSTimer timer;

        public EmotionDetection(MainForm form)
        {
            this.form = form;
        }

        private bool DisplayDeviceConnection(bool state)
        { 
            if (state)
            {
                if (!disconnected) form.UpdateStatus("Device Disconnected");
                disconnected = true;
            }
            else
            {
                if (disconnected) form.UpdateStatus("Device Reconnected");
                disconnected = false;
            }
            return disconnected;
        }

        private void DisplayPicture(PXCMImage image)
        {
            PXCMImage.ImageData data;
            pxcmStatus sts = image.AcquireAccess(PXCMImage.Access.ACCESS_READ, PXCMImage.PixelFormat.PIXEL_FORMAT_RGB24, out data);
            if ( sts >= pxcmStatus.PXCM_STATUS_NO_ERROR)
            {
                form.DisplayBitmap(data.ToBitmap(0, image.info.width, image.info.height));
                timer.Tick("");
                image.ReleaseAccess(data);
            }
        }

        private void DisplayLocation(PXCMEmotion ft)
        {
            int numFaces = ft.QueryNumFaces();
            for (int i=0; i<numFaces;i++) {
                /* Retrieve emotionDet location data */
                PXCMEmotion.EmotionData[] arrData = new PXCMEmotion.EmotionData[form.NUM_EMOTIONS];
                if(ft.QueryAllEmotionData(i, out arrData) >= pxcmStatus.PXCM_STATUS_NO_ERROR){
                    form.DrawLocation(arrData);
                }
            }
        }

        // Handler functions        
        public void SimplePipeline()
        {
            bool sts = true;
            PXCMSenseManager pp = form.session.CreateSenseManager();
            if (pp == null) throw new Exception("Failed to create sense manager");
            disconnected = false;

            /* Set Source & Profile Index */
            PXCMCapture.DeviceInfo info = null;
            if (this.form.GetRecordState())
            {
                pp.captureManager.SetFileName(this.form.GetFileName(), true);
                form.PopulateDeviceMenu();
                if (this.form.Devices.TryGetValue(this.form.GetCheckedDevice(), out info))
                {
                    pp.captureManager.FilterByDeviceInfo(info);
                }
            }
            else if (this.form.GetPlaybackState())
            {
                pp.captureManager.SetFileName(this.form.GetFileName(), false);
            }
            else
            {
                if (this.form.Devices.TryGetValue(this.form.GetCheckedDevice(), out info))
                {
                    pp.captureManager.FilterByDeviceInfo(info);
                }
            }

            /* Set Module */
            pp.EnableEmotion(form.GetCheckedModule());

            /* Initialization */
            form.UpdateStatus("Init Started");

            PXCMSenseManager.Handler handler = new PXCMSenseManager.Handler()
            {
               //GZ onModuleQueryProfile = OnModuleQueryProfile
            };

            if (pp.Init(handler) >= pxcmStatus.PXCM_STATUS_NO_ERROR)
            {
                form.UpdateStatus("Streaming");
                this.timer = new FPSTimer(form);
                PXCMCaptureManager captureManager = pp.QueryCaptureManager();
                if (captureManager == null) throw new Exception("Failed to query capture manager");
                PXCMCapture.Device device = captureManager.QueryDevice();

                if (device != null && !this.form.GetPlaybackState())
                    device.SetDepthConfidenceThreshold(7);
                    //GZ device.SetProperty(PXCMCapture.Device.Property.PROPERTY_DEPTH_CONFIDENCE_THRESHOLD, 7);                

                while (!form.stop)
                {
                    if (pp.AcquireFrame(true) < pxcmStatus.PXCM_STATUS_NO_ERROR) break;
                    if (!DisplayDeviceConnection(!pp.IsConnected()))
                    {
                        /* Display Results */
                        PXCMEmotion ft = pp.QueryEmotion();
                        if (ft == null)
                        {
                            pp.ReleaseFrame();
                            continue;
                        }

                        //GZ DisplayPicture(pp.QueryImageByType(PXCMImage.ImageType.IMAGE_TYPE_COLOR));
                        PXCMCapture.Sample sample = pp.QueryEmotionSample();
                        if (sample == null)
                        {
                            pp.ReleaseFrame();
                            continue;
                        }

                        DisplayPicture(sample.color);

                        DisplayLocation(ft);

                        form.UpdatePanel();
                    }
                    pp.ReleaseFrame();
                }
            }
            else
            {
                form.UpdateStatus("Init Failed");
                sts = false;
            }

            pp.Close();
            pp.Dispose();
            if (sts) form.UpdateStatus("Stopped");
        }
    }
}
