using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace MvcOnlineTicariOtomasyon.Controllers
{
    public class ImportFromExcelController : Controller
    {
        // GET: ImportFromExcel
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ImportExcelFileToDatabase(HttpPostedFileBase FileUpload)
        {
            try
            {
                DataSet ds = new DataSet();
                if (Request.Files["FileUpload"].ContentLength > 0)
                {
                    string fileExtension = System.IO.Path.GetExtension(Request.Files["FileUpload"].FileName);

                    if (fileExtension == ".xls" || fileExtension == ".xlsx")
                    {
                        string fileLocation = Server.MapPath("~/Content/") + Request.Files["FileUpload"].FileName;
                        if (System.IO.File.Exists(fileLocation))
                        {

                            System.IO.File.Delete(fileLocation);
                        }
                        Request.Files["FileUpload"].SaveAs(fileLocation);
                        string excelConnectionString = string.Empty;
                        excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                        fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                        //connection String for xls file format.
                        if (fileExtension == ".xls")
                        {
                            excelConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" +
                            fileLocation + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                        }
                        //connection String for xlsx file format.
                        else if (fileExtension == ".xlsx")
                        {
                            excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                            fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                        }
                        //Create Connection to Excel work book and add oledb namespace
                        OleDbConnection excelConnection = new OleDbConnection(excelConnectionString);
                        excelConnection.Open();
                        DataTable dt = new DataTable();

                        dt = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                        if (dt == null)
                        {
                            return null;
                        }

                        String[] excelSheets = new String[dt.Rows.Count];
                        int t = 0;
                        //excel data saves in temp file here.
                        foreach (DataRow row in dt.Rows)
                        {
                            excelSheets[t] = row["TABLE_NAME"].ToString();
                            t++;
                        }
                        OleDbConnection excelConnection1 = new OleDbConnection(excelConnectionString);


                        string query = string.Format("Select * from [{0}]", excelSheets[0]);
                        using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, excelConnection1))
                        {
                            dataAdapter.Fill(ds);
                        }
                    }
                    if (fileExtension.ToString().ToLower().Equals(".xml"))
                    {
                        string fileLocation = Server.MapPath("~/Content/") + Request.Files["FileUpload"].FileName;
                        if (System.IO.File.Exists(fileLocation))
                        {
                            System.IO.File.Delete(fileLocation);
                        }

                        Request.Files["FileUpload"].SaveAs(fileLocation);
                        XmlTextReader xmlreader = new XmlTextReader(fileLocation);
                        // DataSet ds = new DataSet();
                        ds.ReadXml(xmlreader);
                        xmlreader.Close();
                    }

                    //for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    //{
                    //    string conn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                    //    SqlConnection con = new SqlConnection(conn);
                    //    string query = "Insert into TestExcelCustomerTable(Name,Surname,Tel, Address, Email) Values('" +
                    //    ds.Tables[0].Rows[i][0].ToString() + "','" + ds.Tables[0].Rows[i][1].ToString() +
                    //    "','" + ds.Tables[0].Rows[i][2].ToString() + "', '" + ds.Tables[0].Rows[i][3].ToString() + "', '" + ds.Tables[0].Rows[i][4].ToString() + "')";
                    //    con.Open();
                    //    SqlCommand cmd = new SqlCommand(query, con);
                    //    cmd.ExecuteNonQuery();
                    //    con.Close();
                    //}
                    ViewBag.State = "Success"; ;
                }
                else
                {
                    ViewBag.State = "NoFile";
                }
                return View("Index");

            }
            catch (Exception ex)
            {
                ViewBag.State = "Error";
                return View("Index");
            }
        }
    }
}