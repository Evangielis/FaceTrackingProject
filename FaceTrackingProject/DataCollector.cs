using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading;

namespace DF_FaceTracking.cs
{
    public enum CollectorStatus
    {
        Idle,
        Collecting,
        Finished
    };

    public class DataCollector
    {
        const int SAMPLING_RATE = 200;    //In milliseconds

        public string Name { get; private set; }
        public CollectorStatus Status { get; private set; }

        bool _collecting;
        QuizDataSet _data;
        string _answer;
        int _qID;
        Thread _worker;
        MainForm mform;
        DataTable dataPoints;

        public DataCollector(string Name, int qID, MainForm mform)
        {
            this.Name = Name;
            this.mform = mform;
            this._qID = qID;
            this.Status = CollectorStatus.Idle;
            

            //Data stuff
            this._data = new QuizDataSet();
            this.dataPoints = new DataTable("DATA");
            this.dataPoints.Columns.Add("TimeStamp", typeof(DateTime));
            this.dataPoints.Columns.Add("NumFaces", typeof(int));
            this.dataPoints.Columns.Add("Pulse", typeof(int));
            foreach (PXCMFaceData.ExpressionsData.FaceExpression expr in Enum.GetValues(typeof(PXCMFaceData.ExpressionsData.FaceExpression)))
                this.dataPoints.Columns.Add(expr.ToString(), typeof(Int32));

            //Thread stuff
            this._collecting = false;
            this._worker = new Thread(new ThreadStart(this.DoCollecting));
        }

        public void StartSession()
        {
            if (this._collecting == true)
                throw new InvalidOperationException("Data session started twice.");

            DataRow ur = this._data.Tables["USERS"].NewRow();
            ur["Name"] = this.Name;
            this._data.Tables["USERS"].Rows.Add(ur);

            this.Status = CollectorStatus.Collecting;
            this._collecting = true;
            this._worker.Start();
        }

        public void EndSession(string Answer)
        {
            this._answer = Answer;

            DataRow ar = this._data.Tables["RESPONSES"].NewRow();
            ar["qID"] = this._qID;
            ar["Text"] = this._answer;
            this._data.Tables["RESPONSES"].Rows.Add(ar);

            this._collecting = false;
        }

        public void DoCollecting()
        {
            try
            {
                while(this._collecting || this._answer == null)
                {
                    DataRow dr = this.dataPoints.NewRow();
                    lock (mform.m_expressionStatus)
                    {
                        foreach (var expr in mform.m_expressionStatus.ToList())
                        {
                            dr[expr.Key.ToString()] = expr.Value;
                        }
                    }
                    dr["TimeStamp"] = DateTime.Now;
                    dr["Pulse"] = mform.PulseRate;
                    dr["NumFaces"] = mform.NumFaces;
                    this.dataPoints.Rows.Add(dr);
                    Thread.Sleep(SAMPLING_RATE);
                }
            }
            finally
            {
                this._data.Tables.Add(this.dataPoints);
                this._data.WriteXml("Data\\" + this.Name + "_" + this._qID + ".xml");
                this.Status = CollectorStatus.Finished;
            }
        }
    }
}
