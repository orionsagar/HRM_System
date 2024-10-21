using Domains.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.DAL
{
    public interface IAttendanceDataReader
    {
        public List<RTA_DOWNLOADED> GetFingerPrintAtt(DateTime fromDate, DateTime toDate, string dataSource = null);
    }

    public class ZKTAccessDBHelper : IAttendanceDataReader
    {
        public ZKTAccessDBHelper() { }
        //string constr = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\Att\Att2003.mdb";
        //string constr = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=E:\Ultimate Projects\UltimateHRM\HAMS.mdb";
        string constr = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=D:\Himu\Office\SASM\attan.mdb";

        //public void SaveEmployee(int empID, string CardNo, string Name, string ProximityNo)
        //{
        //    string qstr = "";
        //    try
        //    {
        //        OleDbConnection con = new(constr);
        //        con.Open();
        //        OleDbCommand cmd = new OleDbCommand("SELECT COUNT(Userid) AS ttl FROM Userinfo WHERE (Userid = [UID])", con);
        //        cmd.Parameters.Add("[UID]", OleDbType.VarChar, 500).Value = empID.ToString();
        //        int count = Convert.ToInt32(cmd.ExecuteScalar());

        //        if (count == 0)
        //        {
        //            qstr = "INSERT INTO Userinfo (Userid, Name, IDCard, Cardnum) VALUES  ([UID],[N],[ID],[Card])";
        //            cmd = new OleDbCommand(qstr, con);
        //            cmd.Parameters.Add("[UID]", OleDbType.VarChar, 500).Value = empID.ToString();
        //            cmd.Parameters.Add("[N]", OleDbType.VarChar, 500).Value = Name;
        //            cmd.Parameters.Add("[ID]", OleDbType.VarChar, 500).Value = CardNo;
        //            cmd.Parameters.Add("[Card]", OleDbType.VarChar, 500).Value = ProximityNo;
        //            cmd.ExecuteNonQuery();
        //        }
        //        else
        //        {
        //            qstr = "UPDATE Userinfo SET Name = [N], IDCard = [ID], Cardnum = [Card] WHERE (Userid = [UID])";
        //            cmd = new OleDbCommand(qstr, con);

        //            cmd.Parameters.Add("[N]", OleDbType.VarChar, 500).Value = Name;
        //            cmd.Parameters.Add("[ID]", OleDbType.VarChar, 500).Value = CardNo;
        //            cmd.Parameters.Add("[Card]", OleDbType.VarChar, 500).Value = ProximityNo;
        //            cmd.Parameters.Add("[UID]", OleDbType.VarChar, 500).Value = empID.ToString();
        //            cmd.ExecuteNonQuery();
        //        }

        //        con.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        //public void DeleteEmployee(int empID)
        //{

        //    try
        //    {
        //        OleDbConnection con = new OleDbConnection(constr);
        //        con.Open();
        //        OleDbCommand cmd = new OleDbCommand("DELETE FROM Userinfo WHERE (Userid = [UID])", con);
        //        cmd.Parameters.Add("[UID]", OleDbType.VarChar, 500).Value = empID.ToString();
        //        cmd.ExecuteNonQuery();

        //        con.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //}
        public List<RTA_DOWNLOADED> GetFingerPrintAtt(DateTime fromDate, DateTime toDate, string dataSource = null)
        {
            if (dataSource != null)
            {
                constr = @$"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={dataSource}";
            }

            DataTable dtFPAtt = new();
            try
            {
                //string qstr = "SELECT EventCard AS Userid,cdate(Format(cdate(EventDate),'MM/dd/yyyy') +' '+EventTime) AS CheckTime, DeviceID AS Sensorid,0 AS LogID"
                //              + " FROM PubEvent Checkinout WHERE (((DatePart('d',[EventDate]))=[d]) AND ((DatePart('m',[EventDate]))=[m]) AND ((DatePart('yyyy',[EventDate]))=[y])) order by DeviceID,EventCard";


                //OleDbConnection AccessCon = new OleDbConnection(AccessConStr);
                //isnull(IsKey,'0')='0' AND
                //string qstr = "SELECT Checkinout.VerifyCode as logID,Checkinout.Userid, Checkinout.CheckTime, Checkinout.Sensorid"
                //+ " FROM Checkinout WHERE (((DatePart('d',[Checkinout].[CheckTime]))=[d]) AND ((DatePart('m',[Checkinout].[CheckTime]))=[m]) AND ((DatePart('yyyy',[Checkinout].[CheckTime]))=[y]))";
                //string qstr = "SELECT Checkinout.VerifyCode as logID,USERINFO.badgenumber as UID, Checkinout.CheckTime, Checkinout.Sensorid"
                //+ " FROM Checkinout inner join USERINFO on Clng(USERINFO.badgenumber)=Checkinout.Userid " 
                //+ " WHERE (((DatePart('d',[Checkinout].[CheckTime]))=[d]) AND ((DatePart('m',[Checkinout].[CheckTime]))=[m]) AND ((DatePart('yyyy',[Checkinout].[CheckTime]))=[y]))";

                //string qstr = "SELECT Checkinout.VerifyCode as logID,USERINFO.badgenumber as UID, Checkinout.CheckTime, Checkinout.Sensorid"
                //+ " FROM Checkinout inner join USERINFO on USERINFO.Userid=Checkinout.Userid "
                //+ " WHERE (((DatePart('d',[Checkinout].[CheckTime]))=[d]) AND ((DatePart('m',[Checkinout].[CheckTime]))=[m]) AND ((DatePart('yyyy',[Checkinout].[CheckTime]))=[y]))";
               
                string qstr = "SELECT Checkinout.VerifyCode as logID,USERINFO.badgenumber as UID, Checkinout.CheckTime, Checkinout.Sensorid"
                + " FROM Checkinout inner join USERINFO on USERINFO.Userid=Checkinout.Userid "
                //+ " WHERE (((DatePart('d',[Checkinout].[CheckTime]))=[d]) AND ((DatePart('m',[Checkinout].[CheckTime]))=[m]) AND ((DatePart('yyyy',[Checkinout].[CheckTime]))=[y]))"
                + " WHERE [Checkinout].[CheckTime]>=[fromDate] AND [Checkinout].[CheckTime]<=[toDate]";

                OleDbDataAdapter Ada = new OleDbDataAdapter(qstr, constr);
                //Ada.SelectCommand.Parameters.Add("[d]", OleDbType.Integer).Value = dt.Day;
                //Ada.SelectCommand.Parameters.Add("[m]", OleDbType.Integer).Value = dt.Month;
                //Ada.SelectCommand.Parameters.Add("[y]", OleDbType.Integer).Value = dt.Year;
                Ada.SelectCommand.Parameters.Add("[fromDate]", OleDbType.Date).Value = fromDate;
                Ada.SelectCommand.Parameters.Add("[toDate]", OleDbType.Date).Value = toDate;

                Ada.Fill(dtFPAtt);
                Ada.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            int countRows = dtFPAtt.Rows.Count;
            List<RTA_DOWNLOADED> entitis = new();

            for (int k = 0; k < countRows; k++)
            {
                RTA_DOWNLOADED entity = new();

                entity.FPId = dtFPAtt.Rows[k].Field<string>("UID").ToString();
                //str = dtFPAtt.Rows[k].Field<string>("CheckTime");
                entity.ATT_DATE = dtFPAtt.Rows[k].Field<DateTime>("CheckTime").Date;
                var time = dtFPAtt.Rows[k].Field<DateTime>("CheckTime").TimeOfDay;
                entity.ATT_TIME = new DateTime(1900, 01, 01).Add(time);
                entity.MACHINE_ID = dtFPAtt.Rows[k].Field<string>("Sensorid");
                entity.DOWNLOAD_STATUS = false;
                entitis.Add(entity);
            }

            return entitis;
        }
        //public void UpdateFingerPrintAtt(int LogID)
        //{
        //    try
        //    {
        //        OleDbConnection con = new OleDbConnection(constr);
        //        con.Open();
        //        OleDbCommand cmd = new OleDbCommand("UPDATE PubEvent SET IsKey = 1 WHERE rowAutoID=[LID]", con);
        //        cmd.Parameters.Add("[LID]", OleDbType.Integer).Value = LogID;
        //        cmd.ExecuteNonQuery();

        //        con.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //}

    }
}
