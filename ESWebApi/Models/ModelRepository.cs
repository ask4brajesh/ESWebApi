using Newtonsoft.Json;
using NLog;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace ESWebApi.Models
{
    public class ModelRepository
    {
        public Logger logger = LogManager.GetCurrentClassLogger();

        public DataTable DataTableMaster = null;
        public DataSet DataSet = null;
        public string workspace = ConfigurationManager.AppSettings["geoserverworkspace"];
        public ModelRepository()
        {
            using (NpgsqlConnection connection = new NpgsqlConnection())
            {
                try
                {
                    DataSet = new DataSet();
                    string sql = "select * from master_data";
                    connection.ConnectionString = ConfigurationManager.ConnectionStrings["NpgsqlConnectionString"].ToString();
                    connection.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand(sql, connection);
                    NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(cmd);
                    //DataTableMaster = new DataTable();
                    //connection.Open();
                    adapter.Fill(DataSet, "master_data");
                    cmd = new NpgsqlCommand("select town,nin_id,dtc_name,survey_type,con_to from pole", connection);
                    adapter = new NpgsqlDataAdapter(cmd);
                    adapter.Fill(DataSet, "pole");
                    cmd = new NpgsqlCommand("select town,line_len, fdr_name, survey_type from ht_line", connection);
                    adapter = new NpgsqlDataAdapter(cmd);
                    adapter.Fill(DataSet, "ht_line");
                    cmd = new NpgsqlCommand("select town,line_len, fdr_name, survey_type from ht_cable", connection);
                    adapter = new NpgsqlDataAdapter(cmd);
                    adapter.Fill(DataSet, "ht_cable");

                    cmd = new NpgsqlCommand("select town, survey_type, fdr_name,sur_sta_lt,nin_id,dtr_name from dis_transformer", connection);
                    adapter = new NpgsqlDataAdapter(cmd);
                    adapter.Fill(DataSet, "dis_transformer");

                    cmd = new NpgsqlCommand("select * from town", connection);
                    adapter = new NpgsqlDataAdapter(cmd);
                    adapter.Fill(DataSet, "town");

                    cmd = new NpgsqlCommand("select * from user_master", connection);
                    adapter = new NpgsqlDataAdapter(cmd);
                    adapter.Fill(DataSet, "user_master");

                    cmd = new NpgsqlCommand("select town from fuse", connection);
                    adapter = new NpgsqlDataAdapter(cmd);
                    adapter.Fill(DataSet, "fuse");


                    cmd = new NpgsqlCommand("select nin_id from building", connection);
                    adapter = new NpgsqlDataAdapter(cmd);
                    adapter.Fill(DataSet, "building");

                    cmd = new NpgsqlCommand("select nin_id, town,cab_len, fdr_name, survey_type, dtr_name  from lt_line", connection);
                    adapter = new NpgsqlDataAdapter(cmd);
                    adapter.Fill(DataSet, "lt_line");

                    cmd = new NpgsqlCommand("select nin_id,town,cab_len, fdr_name, survey_type, dtr_name from lt_cable", connection);
                    adapter = new NpgsqlDataAdapter(cmd);
                    adapter.Fill(DataSet, "lt_cable");

                    cmd = new NpgsqlCommand("select * from lkp_surveystatus", connection);
                    adapter = new NpgsqlDataAdapter(cmd);
                    adapter.Fill(DataSet, "lkp_status");


                    cmd = new NpgsqlCommand("select * from consumer", connection);
                    adapter = new NpgsqlDataAdapter(cmd);
                    adapter.Fill(DataSet, "consumer");

                    cmd = new NpgsqlCommand("select * from dtr_master", connection);
                    adapter = new NpgsqlDataAdapter(cmd);
                    adapter.Fill(DataSet, "dtr_master");
                    //adapter.Fill(DataTableMaster);
                    connection.Close();
                }
                catch (Exception ex)
                {
                    logger.Error(" Method : Generate datatable for all table" + ex.Message);
                }

            }

        }
        public Dictionary<string, string> GetUserList()
        {
            List<User> user = new List<User>();

            Dictionary<string, string> keyValues = new Dictionary<string, string>();
            
            try
            {
                DataTable t1 = DataSet.Tables["master_data"];
                string masterdata = JsonConvert.SerializeObject(t1);
                DataTable t2 = DataSet.Tables["user_master"];
                string usermaster = JsonConvert.SerializeObject(t2);
               
                keyValues.Add("usermaster", usermaster);
                keyValues.Add("masterdata", masterdata);
            }
            catch (Exception ex)
            {
                logger.Error(" Method : GetUserList" + ex.Message);
            }
            return keyValues;
        }

        public bool createuser(CreateUser user)
        {
            bool result = false;

            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection())
                {
                    var sql = "Insert into user_master(name,password,usertypeid,emailid,mobileno,adharcardno,isactive,create_date)values('" + user.username + "', '" + user.password + "', '" + user.usertypeid + "', '" + user.emailid + "', '" + user.mobileno + "', '" + user.adharcardno + "', '" + user.isactive + "','" + DateTime.Now + "');";
                    connection.ConnectionString = ConfigurationManager.ConnectionStrings["NpgsqlConnectionString"].ToString();
                    connection.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand(sql, connection);
                    int i = cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    connection.Close();
                    if (i >= 1)
                    {
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                    //result = true;
                }
            }
            catch (Exception ex)
            {
                logger.Error(" Method : createuser" + ex.Message);
                result = false;
            }
            return result;
        }



        public List<Town> GetTownList()
        {
            List<Town> towns = new List<Town>();
            // var userdata = String.Empty;
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection())
                {
                    string sql = "select distinct town from master_data;";
                    connection.ConnectionString = ConfigurationManager.ConnectionStrings["NpgsqlConnectionString"].ToString();
                    connection.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand(sql, connection);
                    NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    //connection.Open();
                    adapter.Fill(dt);
                    connection.Close();

                    foreach (DataRow row in dt.Rows)
                    {
                        towns.Add(
                            new Town
                            {
                                name = Convert.ToString(row["town"])
                            });
                    }
                    cmd.Dispose();
                    connection.Close();

                }
            }
            catch (Exception ex)
            {
                logger.Error(" Method : GetTownList" + ex.Message);
            }
            return towns;
        }

        public List<SurveyType> GetSurveyTypeList()
        {
            List<SurveyType> st = new List<SurveyType>();
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection())
                {
                    string sql = "select * from lkp_surveytype";
                    connection.ConnectionString = ConfigurationManager.ConnectionStrings["NpgsqlConnectionString"].ToString();
                    connection.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand(sql, connection);
                    NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    connection.Close();

                    foreach (DataRow row in dt.Rows)
                    {
                        st.Add(
                            new SurveyType
                            {
                                id = Convert.ToInt32(row["id"]),
                                name = Convert.ToString(row["name"])
                            });
                    }
                    cmd.Dispose();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                logger.Error(" Method : GetSurveyTypeList" + ex.Message);
            }
            return st;
        }



        public List<SubStation> GetSubStaionList(string town, string surveytype = "33")
        {
            List<SubStation> st = new List<SubStation>();
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection())
                {
                    string sql = string.Empty;
                    string column = null;
                    if (surveytype == "33" || surveytype == "33 KV")
                    {
                        sql = "select distinct gss_name_33 from master_data where lower(town) = '" + town.ToLower() + "'";
                        column = "gss_name_33";
                    }
                    else
                    {
                        sql = "select distinct pss_name_11 from master_data where lower(town) = '" + town.ToLower() + "'";
                        column = "pss_name_11";
                    }

                    connection.ConnectionString = ConfigurationManager.ConnectionStrings["NpgsqlConnectionString"].ToString();
                    connection.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand(sql, connection);
                    NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    connection.Close();

                    foreach (DataRow row in dt.Rows)
                    {
                        st.Add(
                            new SubStation
                            {
                                //id = Convert.ToInt32(row["master_id"]),
                                name = Convert.ToString(row[column])
                            });
                    }
                    cmd.Dispose();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                logger.Error(" Method : GetSubStaionList" + ex.Message);
            }
            return st;
        }

        public List<sectionname> GetSecNameList(string town)
        {
            List<sectionname> sectionnames = new List<sectionname>();
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection())
                {
                    string sql = string.Empty;
                    //string column = null;
                    sql = "select distinct section_name from master_data where lower(town) = '" + town.ToLower() + "'";
                   

                    connection.ConnectionString = ConfigurationManager.ConnectionStrings["NpgsqlConnectionString"].ToString();
                    connection.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand(sql, connection);
                    NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    connection.Close();

                    foreach (DataRow row in dt.Rows)
                    {
                        sectionnames.Add(
                            new sectionname
                            {
                                //id = Convert.ToInt32(row["master_id"]),
                                name = Convert.ToString(row["section_name"])
                            });
                    }
                    cmd.Dispose();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                logger.Error(" Method : GetSecNameList" + ex.Message);
            }
            return sectionnames;
        }

        public List<FeederName> GetFeederList(string substation, string fdrtype)
        {
            List<FeederName> st = new List<FeederName>();
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection())
                {
                    string fldFdr_name = string.Empty;
                    string fldFdr_code = string.Empty;
                    string fldss_name = string.Empty;
                    string status_fdr = string.Empty;

                    if (fdrtype == "33")
                    {
                        fldFdr_name = "feeder_name_33";
                        fldFdr_code = "feeder_code_33";
                        fldss_name = "gss_name_33";
                        status_fdr = "status_fdr_33";
                    }

                    if (fdrtype == "11")
                    {
                        fldFdr_name = "feeder_name_11";
                        fldFdr_code = "feeder_code_11";
                        fldss_name = "pss_name_11";
                        status_fdr = "status_fdr_11";
                    }


                    string sql = "select distinct " + fldFdr_name + ", " + fldFdr_code + "," + status_fdr + " from master_data where " + fldss_name + " = '" + substation + "' AND " + status_fdr + "!='Completed';";
                    connection.ConnectionString = ConfigurationManager.ConnectionStrings["NpgsqlConnectionString"].ToString();
                    connection.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand(sql, connection);
                    NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    connection.Close();

                    foreach (DataRow row in dt.Rows)
                    {
                        st.Add(
                            new FeederName
                            {
                                name = Convert.ToString(row[fldFdr_name]),
                                code = Convert.ToString(row[fldFdr_code]),
                                status = Convert.ToString(row[status_fdr])
                            });
                    }
                    cmd.Dispose();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                logger.Error(" Method : GetFeederList" + ex.Message);
            }
            return st;
        }



        public List<AssignTask> GetAssignTaskListbyid(string userid)
        {
            List<AssignTask> assigns = new List<AssignTask>();
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection())
                {
                    //string sql = "select * from User_Allocation_master where userid = '"+ userid + "';";
                    // string sql = "select User_Allocation_master.* from User_Allocation_master inner join user_master on User_Allocation_master.userid = user_master.userid where lower(user_master.name) ='" + userid.ToLower().Trim() + "'";
                    string sql = "select User_Allocation_master.* from User_Allocation_master inner join user_master on User_Allocation_master.userid = user_master.userid where lower(user_master.name) ='" + userid.ToLower().Trim() + "' and User_Allocation_master.surveystatus !=2";

                    connection.ConnectionString = ConfigurationManager.ConnectionStrings["NpgsqlConnectionString"].ToString();
                    connection.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand(sql, connection);
                    NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    connection.Close();
                    foreach (DataRow row in dt.Rows)
                    {
                        string feedertype = Convert.ToString(row["feeder_33"]) == "NA" ? "11" : "33";
                        string fdname = feedertype == "33" ? "feeder_33" : "feeder_11";
                        assigns.Add(
                            new AssignTask
                            {
                                UserId = Convert.ToInt32(row["userid"]),
                                Town = Convert.ToString(row["town"]),
                                SurveyType = Convert.ToString(row["surveytype"]),
                                Substation_33 = Convert.ToString(row["substation_33"]),
                                Substation_11 = Convert.ToString(row["substation_11"]),
                                Feeder_33 = Convert.ToString(row["feeder_33"]),
                                Feeder_11 = Convert.ToString(row["feeder_11"]),
                                Feeder_code = GetFeederCode(Convert.ToString(row[fdname]), feedertype),
                                dt_name = Convert.ToString(row["dt_name"]),
                                SurveyStatus = Convert.ToInt32(row["surveystatus"]),
                                feeder_code = Convert.ToString(row["feeder_code"]),
                                dtr_nin = Convert.ToString(row["dtr_nin"])
                            });
                    }
                    cmd.Dispose();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                logger.Error(" Method : GetAssignTaskListbyid" + ex.Message);
            }
            return assigns;
        }


        public bool AssignUserTask(usertask usertask)
        {
            bool result = false;
            try
            {
                usertask.surveystatus = "2";
                string sql = "INSERT INTO user_allocation_master(userid, town, surveytype, substation_33, substation_11, feeder_33, feeder_11, surveystatus, feeder_code, section_name, dt_name, dtr_nin)	VALUES('" + usertask.userid + "', '" + usertask.town + "', '" + usertask.surveytype + "','" + usertask.substation_33 + "','" + usertask.substation_11 + "','" + usertask.feeder_33 + "', '" + usertask.feeder_11 + "','" + Convert.ToInt32(usertask.surveystatus) + "','" + usertask.feeder_code + "','" + usertask.section_name + "','" + usertask.dtr_name + "','" + usertask.dtr_nin + "'); ";
                string upsql = null;
                if (usertask.surveytype == "33")
                {
                    upsql = "UPDATE public.master_data  SET  status_fdr_33 ='In Progress' WHERE gss_name_33='"+usertask.substation_33+ "' AND feeder_name_33='" + usertask.feeder_33 + "' ; "; 

                }
                else {
                    upsql = "UPDATE public.master_data  SET  status_fdr_11  ='In Progress' WHERE pss_name_11='" + usertask.substation_11 + "' AND feeder_name_='" + usertask.feeder_11 + "' ; ";
                }
               
                using (NpgsqlConnection connection = new NpgsqlConnection())
                {
                    connection.ConnectionString = ConfigurationManager.ConnectionStrings["NpgsqlConnectionString"].ToString();
                    connection.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand(sql, connection);
                    int i = cmd.ExecuteNonQuery();

                    cmd = new NpgsqlCommand(upsql, connection);
                     i = cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    connection.Close();
                    if (i >= 1)
                    {
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }

                }

            }
            catch (Exception ex)
            {
                result = false;
                logger.Error("Method : AssignUserTask : " + ex.Message);
            }
            return result;
        }

        public string GetFeederCode(string feedername, string feedertype = "33")
        {
            string feedercode = string.Empty;
            string fldfeedercode = string.Empty;
            DataRow[] drs = null;
            try
            {
                if (feedertype == "33")
                {
                    drs = DataSet.Tables["master_data"].Select("feeder_name_33 = '" + feedername + "'");
                    fldfeedercode = "feeder_code_33";
                }

                if (feedertype == "11")
                {
                    drs = DataSet.Tables["master_data"].Select("feeder_name_11 = '" + feedername + "'");
                    fldfeedercode = "feeder_code_11";
                }

                if (drs.Length > 0)
                {
                    feedercode = Convert.ToString(drs[0][fldfeedercode]);
                }

            }
            catch (Exception ex)
            {
                logger.Error(" Method : GetFeederCode" + ex.Message);
            }
            return feedercode;
        }

        public Dictionary<string, string> GetUserAssignData() {
            Dictionary<string, string> valuePairs = null;

            return valuePairs;

        }
        public List<featuregeojson> GetGeojsonAsync(string substaionname, string feedername, string surveytype, string town, string dt_name)
        {
            var requestUrl = String.Empty;
            dynamic result = null;
            string[] features = null;


            List<featuregeojson> feature = new List<featuregeojson>();
            string cql_filter = null;
            if (surveytype == "LT")
            {
                features = new string[]{
              "dis_transformer","lt_cable","lt_line","pole"
            };
                cql_filter = "town='" + town + "' AND fdr_name='" + feedername + "' AND substation_name='" + substaionname + "' AND survey_type = '" + surveytype + "'AND strTrim(dtr_name)=strTrim('" + dt_name + "')";
            }

            if (surveytype == "11")
            {
                features = new string[]{
              "dis_transformer","ht_cable","ht_line","pole"
            };
                cql_filter = "town='" + town + "' AND fdr_name='" + feedername + "' AND substation_name='" + substaionname + "' AND survey_type = '" + surveytype + "'";
            }

            if (surveytype == "33")
            {
                features = new string[]{
              "dis_transformer","ht_cable","ht_line","pole" };
                cql_filter = "town='" + town + "' AND fdr_name = '" + feedername + "' AND substation_name='" + substaionname + "' AND survey_type = '" + surveytype + "'";
            }

            if (surveytype == "C")
            {
                features = new string[]{
              "dis_transformer","lt_cable","lt_line","pole","building","consumer" };
                cql_filter = "town='" + town + "' AND fdr_name = '" + feedername + "' AND substation_name='" + substaionname + "' AND dtr_name='" + dt_name + "'";
            }


            for (int i = 0; i < features.Length; i++)
            {
                try
                {
                    result = null;
                    //requestUrl = "http://access.spaceimagingme.com:6093/geoserver/"+ workspace +"/ows?service=WFS&version=1.0.0&request=GetFeature&typeName="+ workspace +":" + features[i] + "&maxFeatures=100&CQL_filter=fdr_name='" + feedername + "' and  substation_name='" + substaionname + "' and survey_type='" + surveytype + "'&outputFormat=application/json";

                    if (features[i] == "pole")
                    {
                        string cql_filter1 = "town='" + town + "' AND fdr_name = '" + feedername + "' AND substation_name='" + substaionname + "' AND dtc_name='" + dt_name + "'";
                        requestUrl = "http://access.spaceimagingme.com:6093/geoserver/" + workspace + "/wfs?service=wfs&version=2.0.0&request=GetFeature&typeNames=" + workspace + ":" + features[i] + "&propertyName=nin_id,geom&cql_filter=" + cql_filter1 + "&outputFormat=application/json";
                    }
                    else if (features[i] == "building")
                    {

                        string cql_filter1 = "city='" + town + "'";
                        requestUrl = "http://access.spaceimagingme.com:6093/geoserver/" + workspace + "/wfs?service=wfs&version=2.0.0&request=GetFeature&typeNames=" + workspace + ":" + features[i] + "&propertyName=nin_id,geom&cql_filter=" + cql_filter1 + "&outputFormat=application/json";
                    }
                    else if (features[i] == "dis_transformer" && surveytype == "LT")
                    {
                        string surveytype1 = "11";
                        string cql_filter1 = "town='" + town + "' AND fdr_name='" + feedername + "' AND substation_name='" + substaionname + "' AND survey_type = '" + surveytype1 + "'AND dtr_name='" + dt_name + "'";
                        requestUrl = "http://access.spaceimagingme.com:6093/geoserver/" + workspace + "/wfs?service=wfs&version=2.0.0&request=GetFeature&typeNames=" + workspace + ":" + features[i] + "&propertyName=nin_id,geom&cql_filter=" + cql_filter1 + "&outputFormat=application/json";
                    }
                    else
                    {
                        requestUrl = "http://access.spaceimagingme.com:6093/geoserver/" + workspace + "/wfs?service=wfs&version=2.0.0&request=GetFeature&typeNames=" + workspace + ":" + features[i] + "&propertyName=nin_id,geom&cql_filter=" + cql_filter + "&outputFormat=application/json";
                    }
                    var req = WebRequest.Create(requestUrl);
                    req.Timeout = 100000;
                    var r = req.GetResponse();
                    var responseReader = new StreamReader(r.GetResponseStream());
                    var responseData = responseReader.ReadToEnd();

                    featuregeojson featuregeojson = new featuregeojson();
                    featuregeojson.layerName = features[i];
                    featuregeojson.geoJson = responseData;

                    feature.Add(featuregeojson);
                }


                catch (Exception ex)
                {
                    logger.Error(" Method : GetGeojsonAsync" + ex.Message);
                }


            }

            //result = Newtonsoft.Json.JsonConvert.DeserializeObject(featuregeojsons);


            return feature;
        }


        public string getCoordsOfDtr(string dtrnin)
        {
            string NinId = null;
            try
            {
                string sql = "select st_x(geom),st_y(geom) from dis_transformer where nin_id='" + dtrnin + "'";
                using (NpgsqlConnection connection = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["NpgsqlConnectionString"].ConnectionString))
                {
                    try
                    {
                        NpgsqlCommand cmd = new NpgsqlCommand(sql, connection);
                        connection.Open();
                        NpgsqlDataReader ndr = cmd.ExecuteReader();
                        if (ndr.HasRows)
                        {
                            if (ndr.Read())
                            {
                                NinId = Convert.ToString(ndr[0]) + "," + Convert.ToString(ndr[1]);
                            }
                        }


                        connection.Close();
                        connection.Dispose();
                    }
                    catch (Exception ex)
                    {
                        NinId = null;
                        logger.Error(" Method : getCoordsOfDtr" + ex.Message);
                    }
                }

            }
            catch (Exception ex)
            {
                NinId = ex.Message;
                logger.Error(" Method : getCoordsOfDtr" + ex.Message);
            }

            return NinId;
        }


        public string getCoordsOfpole(string polenin)
        {
            string NinId = null;
            try
            {
                string sql = "select st_x(geom),st_y(geom) from pole where nin_id='" + polenin + "'";
                using (NpgsqlConnection connection = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["NpgsqlConnectionString"].ConnectionString))
                {
                    try
                    {
                        NpgsqlCommand cmd = new NpgsqlCommand(sql, connection);
                        connection.Open();
                        NpgsqlDataReader ndr = cmd.ExecuteReader();
                        if (ndr.HasRows)
                        {
                            if (ndr.Read())
                            {
                                NinId = Convert.ToString(ndr[0]) + "," + Convert.ToString(ndr[1]);
                            }
                        }


                        connection.Close();
                        connection.Dispose();
                    }
                    catch (Exception ex)
                    {
                        NinId = null;
                        logger.Error(" Method : getCoordsOfpole" + ex.Message);
                    }
                }

            }
            catch (Exception ex)
            {
                NinId = ex.Message;
                logger.Error(" Method : getCoordsOfDtr" + ex.Message);
            }

            return NinId;
        }

        public dtcgeom getDtcGeom(string dtcname)
        {
            dtcgeom dtcgeom = null;
            //string NinId = null;
            string sql = "select st_x(geom),st_y(geom),nin_id from dis_transformer where dt_name='" + dtcname + "'";
            using (NpgsqlConnection connection = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["NpgsqlConnectionString"].ConnectionString))
            {
                try
                {
                    dtcgeom = new dtcgeom();
                    NpgsqlCommand cmd = new NpgsqlCommand(sql, connection);
                    connection.Open();
                    NpgsqlDataReader ndr = cmd.ExecuteReader();
                    if (ndr.HasRows)
                    {
                        if (ndr.Read())
                        {
                            dtcgeom.ninid = Convert.ToString(ndr["nin_id"]);
                            dtcgeom.x = Convert.ToString(ndr[0]);
                            dtcgeom.y = Convert.ToString(ndr[1]);
                        }
                    }


                    connection.Close();
                    connection.Dispose();
                }
                catch (Exception ex)
                {
                    logger.Error(" Method : getDtcGeom" + ex.Message);
                }
            }

            return dtcgeom;
        }

        public List<sectionName> GetSectionList()
        {
            List<sectionName> st = new List<sectionName>();
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection())
                {
                    string sql = "select distinct section_name, section_code from master_data";
                    connection.ConnectionString = ConfigurationManager.ConnectionStrings["NpgsqlConnectionString"].ToString();
                    connection.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand(sql, connection);
                    NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    connection.Close();

                    foreach (DataRow row in dt.Rows)
                    {
                        st.Add(
                            new sectionName
                            {
                                name = Convert.ToString(row["section_name"]),
                                code = Convert.ToString(row["section_code"])

                            });
                    }
                    cmd.Dispose();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                logger.Error(" Method : GetSectionList" + ex.Message);
            }
            return st;
        }




        public string SaveJsonlist(string data, string featuretable)
        {
            string ninid = null;
            var sql1 = String.Empty;
            //string tableName = null;
            //dynamic data1 = null;
            string result = "false";
            if (featuretable == "pole")
            {
                pole data1 = JsonConvert.DeserializeObject<pole>(data);
                ninid = data1.nin_id;
                sql1 = string.Format("Insert into pole (nin_id,fdr_name,sec_name,pre_pol_no,stud_pole,earthing,oth_cable,status,cut_point,op_volt,construct,po_assem,type,net_arran,rising,sec_ins,num_of_ins,type_of_ins,no_stay,trans_ins,crs_arm,st_light,sw_mt_ins,tp_str_lig,st_fit_wat,st_fit_typ,li_arr_ins,sur_remark,create_by,create_date,substation_name,survey_type,town,con_to,dtc_name,is_shared,no_str_lig,cro_arm_type, geom)values('" + data1.nin_id + "', '" + data1.fdr_name + "', '" + data1.sec_name + "','" + data1.pre_pol_no + "', '" + data1.stud_pole + "', '" + data1.earthing + "', '" + data1.oth_cable + "','" + data1.status + "','" + data1.cut_point + "','" + data1.op_volt + "','" + data1.construct + "','" + data1.po_assem + "','" + data1.type + "','" + data1.net_arran + "','" + data1.rising + "','" + data1.sec_ins + "','" + data1.num_of_ins + "','" + data1.type_of_ins + "','" + data1.no_stay + "','" + data1.trans_ins + "','" + data1.crs_arm + "','" + data1.st_light + "','" + data1.sw_mt_ins + "','" + data1.tp_str_lig + "','" + data1.st_fit_wat + "','" + data1.st_fit_typ + "','" + data1.li_arr_ins + "','" + data1.sur_remark + "','" + data1.create_By + "','" + data1.create_Date + "','" + data1.substation_name + "','" + data1.survey_type + "','" + data1.town + "','" + data1.con_to + "','" + data1.dtc_name + "','" + data1.is_shared + "','" + data1.no_str_lig + "','"+data1.cro_arm_type + "',ST_SetSRID(ST_MakePoint(" + data1.x + "," + data1.y + "),4326));");
            }
            else if (featuretable == "ht_cable")
            {
                ht_cable data1 = JsonConvert.DeserializeObject<ht_cable>(data);
                ninid = data1.nin_id;
                sql1 = "Insert into ht_cable (op_volt, fdr_name, status, phase, ins_type, cr_sec_ar, no_core, cab_mat, con_pos, cab_typ, met_no, nin_id, pre_pol_no,sec_name, sur_remark, create_by,create_date, substation_name, survey_type,town, geom,line_len)values('" + data1.op_volt + "', '" + data1.fdr_name + "', '" + data1.status + "', '" + data1.phase + "', '" + data1.ins_type + "', '" + data1.cr_sec_ar + "', '" + data1.no_core + "','" + data1.cab_mat + "','" + data1.con_pos + "','" + data1.cab_typ + "','" + data1.met_no + "','" + data1.nin_id + "','" + data1.pre_pol_no + "','" + data1.sec_name + "','" + data1.sur_remark + "','" + data1.create_by + "','" + data1.create_date + "','" + data1.substation_name + "','" + data1.survey_type + "','" + data1.town + "',ST_SetSRID(ST_GeomFromText('LINESTRING(" + data1.from_x + " " + data1.from_y + "," + data1.to_x + " " + data1.to_y + ")'),4326),ST_Length(ST_Transform(ST_SetSRID(ST_GeomFromText('LINESTRING(" + data1.from_x + " " + data1.from_y + "," + data1.to_x + " " + data1.to_y + ")'),4326),26986)));";
            }
            else if (featuretable == "ht_line")
            {
                ht_line data1 = JsonConvert.DeserializeObject<ht_line>(data);
                //
                ninid = data1.nin_id;
                sql1 = "Insert into ht_line (nin_id, op_volt, fdr_name, pre_pol_no, status, no_of_wire, phase, ins_type, cond_conf, cond_type,sec_name, sur_remark, create_by, create_date, substation_name, survey_type,town, geom,line_len)values('" + data1.nin_id + "','" + data1.op_volt + "', '" + data1.fdr_name + "', '" + data1.pre_pol_no + "','" + data1.status + "', '" + data1.no_of_wire + "','" + data1.phase + "', '" + data1.ins_type + "', '" + data1.cond_conf + "', '" + data1.cond_type + "','" + data1.sec_name + "','" + data1.sur_remark + "','" + data1.create_by + "','" + data1.create_date + "','" + data1.substation_name + "','" + data1.survey_type + "','" + data1.town + "',ST_SetSRID(ST_GeomFromText('LINESTRING(" + data1.from_x + " " + data1.from_y + "," + data1.to_x + " " + data1.to_y + ")'),4326),ST_Length(ST_Transform(ST_SetSRID(ST_GeomFromText('LINESTRING(" + data1.from_x + " " + data1.from_y + "," + data1.to_x + " " + data1.to_y + ")'),4326),26986)));";
            }
            else if (featuretable == "feeder_pillar")
            {
                feeder_pillar data1 = JsonConvert.DeserializeObject<feeder_pillar>(data);
                ninid = data1.nin_id;
                sql1 = "Insert into feeder_pillar (nin_id,fdr_name,sec_name,pre_pol_no,earthing,x,y,status,op_volt,construct,type,no_of_w_fp,sur_remark,create_by,create_date,substation_name,survey_type,town,dtc_name,geom)values('" + data1.nin_id + "', '" + data1.fdr_name + "', '" + data1.sec_name + "', '" + data1.pre_pol_no + "', '" + data1.earthing + "', '" + data1.x + "', '" + data1.y + "','" + data1.status + "','" + data1.op_volt + "','" + data1.construct + "','" + data1.type + "','" + data1.no_of_w_fp + "','" + data1.sur_remark + "','" + data1.create_by + "','" + data1.create_date + "','" + data1.substation_name + "','" + data1.survey_type + "','" + data1.town + "','" + data1.dtc_name + "',ST_SetSRID(ST_MakePoint(" + data1.x + "," + data1.y + "),4326));";
            }
            else if (featuretable == "service_pillar")
            {
                service_pillar data1 = JsonConvert.DeserializeObject<service_pillar>(data);
                ninid = data1.nin_id;
                sql1 = "Insert into service_pillar (nin_id,fdr_name,sec_name,pre_pol_no,earthing,status,op_volt,construct,type,no_of_w_fp,sur_remark,create_by,create_date,substation_name,survey_type,town,dtc_name,geom)values('" + data1.nin_id + "', '" + data1.fdr_name + "', '" + data1.sec_name + "', '" + data1.pre_pol_no + "', '" + data1.earthing + "','" + data1.status + "','" + data1.op_volt + "','" + data1.construct + "','" + data1.type + "','" + data1.no_of_w_fp + "','" + data1.sur_remark + "','" + data1.create_by + "','" + data1.create_date + "','" + data1.substation_name + "','" + data1.survey_type + "','" + data1.town + "','" + data1.dtc_name + "',ST_SetSRID(ST_MakePoint(" + data1.x + "," + data1.y + "),4326));";
            }
            else if (featuretable == "earth_switch")
            {
                earth_switch data1 = JsonConvert.DeserializeObject<earth_switch>(data);
                ninid = data1.nin_id;
                sql1 = "Insert into earth_switch (nin_id, conn, status, op_volt, man_name, man_s_no, man_date, com_date, phasing, rat_curr,nor_rat_cu, ear_sw_typ, sur_remark, create_by, create_date, substation_name,	survey_type, sec_name, fdr_name,town, geom)values('" + data1.nin_id + "', '" + data1.conn + "', '" + data1.status + "', '" + data1.op_volt + "', '" + data1.man_name + "','" + data1.man_s_no + "'," + validateDate(data1.man_date) + "," + validateDate(data1.com_date) + ",'" + data1.phasing + "','" + data1.rat_curr + "','" + data1.nor_rat_cu + "','" + data1.ear_sw_typ + "','" + data1.sur_remark + "','" + data1.create_by + "','" + data1.create_date + "','" + data1.substation_name + "','" + data1.survey_type + "','" + data1.sec_name + "','" + data1.fdr_name + "','" + data1.town + "',ST_SetSRID(ST_MakePoint(" + data1.x + "," + data1.y + "),4326));";
            }
            else if (featuretable == "dis_transformer")
            {

                dis_transformer data1 = JsonConvert.DeserializeObject<dis_transformer>(data);
                if (data1.survey_type == "11") {
                    bool tempDtName = UpdateDtrStatus(data1.fdr_name, data1.substation_name, data1.dt_name);
                }
                
                ninid = data1.nin_id;
                sql1 = string.Format("INSERT INTO public.dis_transformer( nin_id, con_to_pol, earthing, ear_type, dtr_code, st_name, status, man_name, man_s_no, man_date, dtr_name, fencing, type, sub_type, usage, capacity, hi_v_si_v, lo_v_si_v, cor_typ, vec_grp, per_imp, lv_fdr_pr, hv_pro, phase, lv_pro, own_by, no_of_lt_p, no_oflt_fd, met_no, mod_num, pre_of_tap, cur_tap_po, tap_ch_typ, min_tap, max_tap, tp_ch_m_nm, tp_ch_s_no, tr_nm_phot, geom, sur_remark, create_by, create_date, substation_name, survey_type, sec_name, town,fdr_name,image,image_ext)VALUES('" + data1.nin_id + "','" + data1.con_to_pol + "','" + data1.earthing + "', '" + data1.ear_type + "', '" + data1.dtr_code + "', '" + data1.st_name + "', '" + data1.status + "', '" + data1.man_name + "', '" + data1.man_s_no + "', " + validateDate(data1.man_date) + ", '" + data1.dt_name + "', '" + data1.fencing + "', '" + data1.type + "', '" + data1.sub_type + "', '" + data1.usage + "', '" + data1.capacity + "', '" + data1.hi_v_si_v + "', '" + data1.lo_v_si_v + "', '" + data1.cor_typ + "', '" + data1.vec_grp + "', '" + data1.per_imp + "', '" + data1.lv_fdr_pr + "', '" + data1.hv_pro + "', '" + data1.phase + "', '" + data1.lv_pro + "', '" + data1.own_by + "', '" + data1.no_of_lt_p + "', '" + data1.no_oflt_fd + "', '" + data1.met_no + "', '" + data1.mod_num + "', '" + data1.pre_of_tap + "', '" + data1.cur_tap_po + "', '" + data1.tap_ch_typ + "', '" + data1.min_tap + "', '" + data1.max_tap + "', '" + data1.tp_ch_m_nm + "', '" + data1.tp_ch_s_no + "', '" + data1.tr_nm_phot + "',ST_SetSRID(ST_MakePoint(" + data1.x + "," + data1.y + "),4326), '" + data1.sur_remark + "', '" + data1.create_by + "', '" + data1.create_date + "', '" + data1.substation_name + "', '" + data1.survey_type + "', '" + data1.sec_name + "','" + data1.town + "', '" + data1.fdr_name + "','" + data1.image + "', '" + data1.image_ext + "');");
            }
            else if (featuretable == "dis_box")
            {
                dis_box data1 = JsonConvert.DeserializeObject<dis_box>(data);
                ninid = data1.nin_id;
                sql1 = "Insert into dis_box( st_name, status, man_name, man_s_no, man_date, com_date, op_volt, rat_curr, phase, db_type,sec_name,fdr_name, sur_remark, create_by, create_date, substation_name, survey_type, nin_id,town,geom)values('" + data1.st_name + "', '" + data1.status + "', '" + data1.man_name + "', '" + data1.man_s_no + "', " + validateDate(data1.man_date) + ", " + validateDate(data1.com_date) + ",'" + data1.op_volt + "','" + data1.rat_curr + "','" + data1.phase + "','" + data1.db_type + "','" + data1.sec_name + "','" + data1.fdr_name + "','" + data1.sur_remark + "','" + data1.create_by + "','" + data1.create_date + "','" + data1.substation_name + "','" + data1.survey_type + "','" + data1.nin_id + "','" + data1.town + "',ST_SetSRID(ST_MakePoint(" + data1.x + "," + data1.y + "),4326));";
            }
            else if (featuretable == "rmu")
            {
                rmu data1 = JsonConvert.DeserializeObject<rmu>(data);
                ninid = data1.nin_id;
                sql1 = "Insert into rmu(con_to_pol, sec_name, earthing, st_name, status, man_name, man_s_no, man_date, rmu_type, rmu_name, op_volt, tr_nm_phot, nin_id,fdr_name,sur_remark, create_by, create_date, substation_name, survey_type,town,geom)values('" + data1.con_to_pol + "', '" + data1.sec_name + "', '" + data1.earthing + "', '" + data1.st_name + "', '" + data1.status + "', '" + data1.man_name + "','" + data1.man_s_no + "'," + validateDate(data1.man_date) + ",'" + data1.rmu_type + "','" + data1.rmu_name + "','" + data1.op_volt + "','" + data1.tr_nm_phot + "','" + data1.nin_id + "','" + data1.fdr_name + "','" + data1.sur_remark + "','" + data1.create_by + "','" + data1.create_date + "','" + data1.substation_name + "','" + data1.survey_type + "','" + data1.town + "',ST_SetSRID(ST_MakePoint(" + data1.x + "," + data1.y + "),4326));";
            }
            else if (featuretable == "fuse")
            {
                fuse data1 = JsonConvert.DeserializeObject<fuse>(data);
                ninid = data1.nin_id;
                sql1 = "Insert into fuse (connected, status, op_volt, type, fdr_name, phasing, rat_curr,sec_name, sur_remark, create_by, create_date, substation_name, survey_type, nin_id,town,dtc_name,geom)values('" + data1.connected + "', '" + data1.status + "', '" + data1.op_volt + "', '" + data1.type + "', '" + data1.fdr_name + "', '" + data1.phasing + "','" + data1.rat_curr + "','" + data1.sec_name + "','" + data1.sur_remark + "','" + data1.create_by + "','" + data1.create_date + "','" + data1.substation_name + "','" + data1.survey_type + "','" + data1.nin_id + "','" + data1.town + "','" + data1.dtc_name + "',ST_SetSRID(ST_MakePoint(" + data1.x + "," + data1.y + "),4326));";
            }
            else if (featuretable == "meter")
            {
                meter data1 = JsonConvert.DeserializeObject<meter>(data);
                ninid = data1.nin_id;
                //var date = data1.com_date == '' ? 'NULL' : data1.com_date;
                sql1 = "Insert into meter(connected, status, op_volt, man_name, man_s_no, man_date, com_date, type, ca_no, mul_fac, con_type, met_no, ct_rat, pt_rat, purpose, mdi, pow_fac,sec_name,fdr_name, sur_remark, create_by, create_date, substation_name, survey_type, nin_id,town,geom)values('" + data1.connected + "','" + data1.status + "', '" + data1.op_volt + "', '" + data1.man_name + "', '" + data1.man_s_no + "', " + validateDate(data1.man_date) + "," + validateDate(data1.com_date) + ",'" + data1.type + "','" + data1.ca_no + "','" + data1.mul_fac + "','" + data1.con_type + "','" + data1.met_no + "','" + data1.ct_rat + "','" + data1.pt_rat + "','" + data1.purpose + "','" + data1.mdi + "','" + data1.pow_fac + "','" + data1.sec_name + "','" + data1.fdr_name + "','" + data1.sur_remark + "','" + data1.create_by + "','" + data1.create_date + "','" + data1.substation_name + "','" + data1.survey_type + "','" + data1.nin_id + "','" + data1.town + "',ST_SetSRID(ST_MakePoint(" + data1.x + "," + data1.y + "),4326));";
            }
            else if (featuretable == "lig_arrester")
            {
                lig_arrester data1 = JsonConvert.DeserializeObject<lig_arrester>(data);
                ninid = data1.nin_id;
                sql1 = "Insert into lig_arrester(connected, status, op_volt, man_name, man_s_no, man_date, com_date, type, purpose, dis_cur_cp, phasing,sec_name,fdr_name, sur_remark, create_by, create_date, substation_name, survey_type, nin_id,town,geom)values('" + data1.connected + "', '" + data1.status + "', '" + data1.op_volt + "', '" + data1.man_name + "', '" + data1.man_s_no + "', " + validateDate(data1.man_date) + "," + validateDate(data1.com_date) + ",'" + data1.type + "','" + data1.purpose + "','" + data1.dis_cur_cp + "','" + data1.phasing + "','" + data1.sec_name + "','" + data1.fdr_name + "','" + data1.sur_remark + "','" + data1.create_by + "','" + data1.create_date + "','" + data1.substation_name + "','" + data1.survey_type + "','" + data1.nin_id + "','" + data1.town + "',ST_SetSRID(ST_MakePoint(" + data1.x + "," + data1.y + "),4326));";
            }
            else if (featuretable == "ct")
            {
                ct data1 = JsonConvert.DeserializeObject<ct>(data);
                ninid = data1.nin_id;
                sql1 = "Insert into ct (st_name, status, op_volt, phy_loc, man_name, man_s_no, man_date, com_date, con_type, ct_type, no_of_core, ratio,sec_name,fdr_name, sur_remark, create_by, create_date, substation_name, survey_type, nin_id,town,geom)values('" + data1.st_name + "', '" + data1.status + "', '" + data1.op_volt + "',  '" + data1.phy_loc + "','" + data1.man_name + "', '" + data1.man_s_no + "', " + validateDate(data1.man_date) + "," + validateDate(data1.com_date) + ",'" + data1.con_type + "','" + data1.ct_type + "','" + data1.no_of_core + "','" + data1.ratio + "','" + data1.sec_name + "','" + data1.fdr_name + "','" + data1.sur_remark + "','" + data1.create_by + "','" + data1.create_date + "','" + data1.substation_name + "','" + data1.survey_type + "','" + data1.nin_id + "','" + data1.town + "',ST_SetSRID(ST_MakePoint(" + data1.x + "," + data1.y + "),4326));";
            }
            else if (featuretable == "pt")
            {
                pt data1 = JsonConvert.DeserializeObject<pt>(data);
                ninid = data1.nin_id;
                sql1 = "Insert into pt(st_name, status, op_volt, phy_loc, man_name, man_s_no, man_date, com_date, con_type, pt_type, no_of_core, ratio,sec_name,fdr_name, sur_remark, create_by, create_date, substation_name, survey_type, nin_id,town,geom)values('" + data1.st_name + "', '" + data1.status + "', '" + data1.op_volt + "',  '" + data1.phy_loc + "','" + data1.man_name + "', '" + data1.man_s_no + "', " + validateDate(data1.man_date) + "," + validateDate(data1.com_date) + ",'" + data1.con_type + "','" + data1.pt_type + "','" + data1.no_of_core + "','" + data1.ratio + "','" + data1.sec_name + "','" + data1.fdr_name + "','" + data1.sur_remark + "','" + data1.create_by + "','" + data1.create_date + "','" + data1.substation_name + "','" + data1.survey_type + "','" + data1.nin_id + "','" + data1.town + "',ST_SetSRID(ST_MakePoint(" + data1.x + "," + data1.y + "),4326));";
            }

            else if (featuretable == "tower_33")
            {
                tower_33 data1 = JsonConvert.DeserializeObject<tower_33>(data);
                ninid = data1.nin_id;

                sql1 = "INSERT INTO tower_33(nin_id, fdr_name, sec_name, pre_pol_no, earthing, status, op_volt, construct, type, sec_ins, num_of_ins, type_of_ins, no_circ, he_of_tow, sur_remark, create_by, create_date, substation_name, survey_type, town, geom)VALUES('" + data1.nin_id + "', '" + data1.fdr_name + "', '" + data1.sec_name + "', '" + data1.pre_pol_no + "', '" + data1.earthing + "', '" + data1.status + "', '" + data1.op_volt + "', '" + data1.construct + "', '" + data1.type + "', '" + data1.sec_ins + "', '" + data1.num_of_ins + "', '" + data1.typ_insu + "', '" + data1.no_circ + "', '" + data1.he_of_tow + "', '" + data1.sur_remark + "', '" + data1.create_by + "', '" + data1.create_date + "', '" + data1.substation_name + "', '" + data1.survey_type + "', '" + data1.town + "',ST_SetSRID(ST_MakePoint(" + data1.x + "," + data1.y + "),4326));";
            }

            else if (featuretable == "lt_cable")
            {
                lt_cable data1 = JsonConvert.DeserializeObject<lt_cable>(data);
                ninid = data1.nin_id;
                sql1 = "INSERT INTO lt_cable( nin_id, pre_pol_no, op_volt, fdr_name, status, phase, ins_type, cr_sec_ar, no_core, cab_mat, con_pos, cab_typ, met_no, sur_remark, dtr_name, create_by, create_date, substation_name, survey_type,con_to,town, geom,cab_len)VALUES( '" + data1.nin_id + "', '" + data1.pre_pol_no + "','" + data1.op_volt + "', '" + data1.fdr_name + "', '" + data1.status + "', '" + data1.phase + "', '" + data1.ins_type + "', '" + data1.cr_sec_ar + "', '" + data1.no_core + "', '" + data1.cab_mat + "', '" + data1.con_pos + "', '" + data1.cab_typ + "', '" + data1.met_no + "', '" + data1.sur_remark + "', '" + data1.dtr_name + "', '" + data1.create_by + "', '" + data1.create_date + "', '" + data1.substation_name + "', '" + data1.survey_type + "', '" + data1.con_to + "', '" + data1.town + "',ST_SetSRID(ST_GeomFromText('LINESTRING(" + data1.from_x + " " + data1.from_y + "," + data1.to_x + " " + data1.to_y + ")'),4326),ST_Length(ST_Transform(ST_SetSRID(ST_GeomFromText('LINESTRING(" + data1.from_x + " " + data1.from_y + "," + data1.to_x + " " + data1.to_y + ")'),4326),26986)));";
            }

            else if (featuretable == "lt_line")
            {
                lt_line data1 = JsonConvert.DeserializeObject<lt_line>(data);
                ninid = data1.nin_id;
                sql1 = "INSERT INTO lt_line(nin_id, op_volt, fdr_name, pre_pol_no, status, no_of_wire, phase, ins_type,    cond_conf, c_c_type, sur_remark, dtr_name, create_by, create_date, substation_name,survey_type,con_to, town, geom,cab_len)VALUES( '" + data1.nin_id + "', '" + data1.op_volt + "','" + data1.fdr_name + "', '" + data1.pre_pol_no + "', '" + data1.status + "', '" + data1.no_of_wire + "', '" + data1.phase + "', '" + data1.ins_type + "', '" + data1.cond_conf + "', '" + data1.c_c_type + "', '" + data1.sur_remark + "', '" + data1.dtr_name + "', '" + data1.create_by + "', '" + data1.create_date + "', '" + data1.substation_name + "', '" + data1.survey_type + "', '" + data1.con_to + "', '" + data1.town + "',ST_SetSRID(ST_GeomFromText('LINESTRING(" + data1.from_x + " " + data1.from_y + "," + data1.to_x + " " + data1.to_y + ")'),4326),ST_Length(ST_Transform(ST_SetSRID(ST_GeomFromText('LINESTRING(" + data1.from_x + " " + data1.from_y + "," + data1.to_x + " " + data1.to_y + ")'),4326),26986)));";
            }


            try
            {

                //dynamic SRID = 4326; POINT(78.548333 17.417381);
                using (NpgsqlConnection connection = new NpgsqlConnection())
                {


                    connection.ConnectionString = ConfigurationManager.ConnectionStrings["NpgsqlConnectionString"].ToString();
                    connection.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "Select count(*) from " + featuretable + " where nin_id = '" + ninid + "'";
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    //NpgsqlCommand cmd = new NpgsqlCommand(sql1, connection);
                    if (count == 0)
                    {
                        cmd = new NpgsqlCommand(sql1, connection);
                        int i = cmd.ExecuteNonQuery();
                        cmd.Dispose();
                        connection.Close();
                        if (i >= 1)
                        {
                            result = "true";
                        }
                        else
                        {
                            result = "false";
                        }
                    }
                    else
                    {
                        result = "Nin Id Already exists";
                    }

                    //result = true;
                }
            }
            catch (Exception ex)
            {
                logger.Error(" Method : SaveJsonList" + ex.Message);
                result = "false + ex" + ex.Message;
            }
            return result;
        }

        public string getNinIdByFeature(string substionName, string feederName, string surveryType, string featureType, string dtcname = "NA", string con_to = "NA", string curr_pole = "NA")
        {
            DataRow[] dr = null;
            string feederCodeBase = null;
            string col_feedercode = null;
            string NinId = null;
            string ninIdPrx = null;
            string circuit = null;
            if (dtcname != "NA" && surveryType == "C")
            {
                DataRow[] rows = DataSet.Tables["lt_cable"].Select("survey_type = 'C'");
                if (rows.Count() == 0)
                {
                    NinId = "C0000001";
                }
                else
                {
                    string tmp_ninid = Convert.ToString(rows[rows.Length - 1]["nin_id"]);
                    int data = Convert.ToInt32(tmp_ninid.Substring(tmp_ninid.Length - 7)) + 1;
                    string part1 = data.ToString().PadLeft(7, '0');
                    NinId = "C" + part1;
                }

                return NinId;

            }
            if (dtcname != "NA" && surveryType == "LT")
            {
                if (featureType == "lt_line" || featureType == "lt_cable")
                {
                    DataRow[] rows = DataSet.Tables["dis_transformer"].Select("dtr_name='" + dtcname + "'");
                    string dtrid = Convert.ToString(rows[0]["nin_id"]);
                    string cpole = curr_pole;
                    string dtid = cpole.Substring(0, cpole.Length - 4);
                    DataRow[] prows = DataSet.Tables[featureType].Select("nin_id LIKE '%" + dtid + "%'");
                    if (prows.Length == 0)
                    {
                        if (featureType == "lt_line")
                        {
                            NinId = dtid + "L001";
                        }

                        if (featureType == "lt_cable")
                        {
                            NinId = dtid + "C001";
                        }
                    }
                    else
                    {
                        string lprow = Convert.ToString(prows[prows.Length - 1]["nin_id"]);
                        // "432122403D0002AL001"
                        int tmpnin = Convert.ToInt32(lprow.Substring(lprow.Length - 3)) + 1;
                        if (featureType == "lt_line")
                        {
                            NinId = dtid + "L" + tmpnin.ToString().PadLeft(3, '0');
                        }

                        if (featureType == "lt_cable")
                        {
                            NinId = dtid + "C" + tmpnin.ToString().PadLeft(3, '0');
                        }

                    }

                    return NinId;
                }

                if (featureType == "pole")
                {
                    DataRow[] rows = DataSet.Tables["dis_transformer"].Select("dtr_name='" + dtcname + "'");
                    string dtrid = Convert.ToString(rows[0]["nin_id"]);
                    if (con_to == "POLE")
                    {
                        DataRow[] dr1 = DataSet.Tables["pole"].Select("dtc_name='" + dtcname + "' and survey_type='" + surveryType + "'");
                        if (dr1.Length > 0)
                        {
                            string tmp_ninid = Convert.ToString(dr1[dr1.Length - 1]["nin_id"]);
                            int polenin = Convert.ToInt32(tmp_ninid.Substring(tmp_ninid.Length - 3)) + 1;
                            string part1 = tmp_ninid.Substring(0, tmp_ninid.Length - 3);
                            string part2 = polenin.ToString().PadLeft(3, '0');
                            NinId = part1 + part2;
                        }
                        else
                        {
                            NinId = dtrid + "A" + "P001";
                        }

                    }
                    if (con_to == "DTR")
                    {
                        int cntpole = DataSet.Tables["pole"].Select("dtc_name='" + dtcname + "' and con_to='" + con_to + "'").Count();

                        if (cntpole == 0)
                        {
                            circuit = "A";
                        }
                        else if (cntpole == 1)
                        {
                            circuit = "B";
                        }
                        else if (cntpole == 2)
                        {
                            circuit = "C";
                        }
                        else if (cntpole == 3)
                        {
                            circuit = "D";
                        }
                        else if (cntpole == 4)
                        {
                            circuit = "E";
                        }
                        else if (cntpole == 5)
                        {
                            circuit = "F";
                        }

                        NinId = dtrid + circuit + "P001";
                    }

                    return NinId;
                }

                if (featureType == "service_pillar" || featureType == "fuse" || featureType == "feeder_pillar")
                {

                    dr = DataSet.Tables["master_data"].Select("feeder_name_11 = '" + feederName + "' and pss_name_11 = '" + substionName + "'");
                    col_feedercode = "feeder_code_11";

                    if (dr.Length == 0)
                    {
                        return NinId = null;
                    }
                    else
                    {
                        feederCodeBase = Convert.ToString(dr[0][col_feedercode]);
                        if (!String.IsNullOrEmpty(feederCodeBase))
                        {
                            int resultcount = Convert.ToInt32(getFeatureCountByTable(substionName, feederName, surveryType, featureType));
                            if (resultcount == 0)
                            {


                                if (featureType == "service_pillar") { NinId = feederCodeBase + "SP001"; ninIdPrx = "SP"; }
                                else if (featureType == "feeder_pillar") { NinId = feederCodeBase + "FP001"; ninIdPrx = "FP"; }
                                else if (featureType == "fuse") { NinId = feederCodeBase + "F0001"; ninIdPrx = "F"; }
                            }
                            else
                            {

                                if (featureType == "service_pillar") { ninIdPrx = "SP"; }
                                else if (featureType == "feeder_pillar") { ninIdPrx = "FP"; }
                                else if (featureType == "fuse") { ninIdPrx = "F"; }

                                string sql = "select nin_id from " + featureType + " order by id desc limit 1";

                                using (NpgsqlConnection connection = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["NpgsqlConnectionString"].ConnectionString))
                                {
                                    try
                                    {
                                        NpgsqlCommand cmd = new NpgsqlCommand(sql, connection);
                                        connection.Open();
                                        NpgsqlDataReader ndr = cmd.ExecuteReader();
                                        if (ndr.Read())
                                        {
                                            string tmp_ninid = (string)ndr["nin_id"];

                                            if (ninIdPrx == "LA" || ninIdPrx == "CT" || ninIdPrx == "PT" || ninIdPrx == "DB" || ninIdPrx == "SP" || ninIdPrx == "FP" || ninIdPrx == "PS" || ninIdPrx == "ES" || ninIdPrx == "TW" || ninIdPrx == "LL" || ninIdPrx == "LC")
                                            {
                                                int data = Convert.ToInt32(tmp_ninid.Substring(tmp_ninid.Length - 3)) + 1;
                                                string part1 = data.ToString().PadLeft(3, '0');
                                                //int part2 = Convert.ToInt32(NinId.Substring(data, NinId.Length) + 1);
                                                NinId = feederCodeBase + ninIdPrx + part1;
                                            }
                                            else
                                            {
                                                int data = Convert.ToInt32(tmp_ninid.Substring(tmp_ninid.Length - 4)) + 1;
                                                string part1 = data.ToString().PadLeft(4, '0');
                                                //int part2 = Convert.ToInt32(NinId.Substring(data, NinId.Length) + 1);
                                                NinId = feederCodeBase + ninIdPrx + part1;
                                            }

                                        }
                                        connection.Close();
                                    }
                                    catch (Exception ex)
                                    {
                                        NinId = null;
                                    }
                                }

                                // string tmp_ninid = "432114101P0001";
                            }
                        }

                        return NinId;
                    }

                }


            }


            if (featureType == "building")
            {
                int cntbld = DataSet.Tables["building"].Rows.Count;
                if (cntbld == 0)
                {
                    NinId = "BLD0000001";
                }
                else
                {
                    DataRow dr1 = DataSet.Tables["building"].Rows[cntbld - 1];
                    string tmp_ninid = Convert.ToString(dr1["nin_id"]);
                    int data = Convert.ToInt32(tmp_ninid.Substring(tmp_ninid.Length - 7)) + 1;
                    string part1 = data.ToString().PadLeft(7, '0');
                    NinId = "BLD" + part1;
                }
                return NinId;
            }

            if (featureType == "consumer")
            {
                int cntbld = DataSet.Tables["consumer"].Rows.Count;
                if (cntbld == 0)
                {
                    NinId = "CON0000001";
                }
                else
                {
                    DataRow dr1 = DataSet.Tables["consumer"].Rows[cntbld - 1];
                    string tmp_ninid = Convert.ToString(dr1["nin_id"]);
                    int data = Convert.ToInt32(tmp_ninid.Substring(tmp_ninid.Length - 7)) + 1;
                    string part1 = data.ToString().PadLeft(7, '0');
                    NinId = "CON" + part1;
                }
                return NinId;
            }

            if (surveryType == "33")
            {
                dr = DataSet.Tables["master_data"].Select("feeder_name_33 = '" + feederName + "' and gss_name_33 = '" + substionName + "'");
                col_feedercode = "feeder_code_33";
            }
            else if (surveryType == "11")
            {
                dr = DataSet.Tables["master_data"].Select("feeder_name_11 = '" + feederName + "' and pss_name_11 = '" + substionName + "'");
                col_feedercode = "feeder_code_11";
            }

            if (dr.Length == 0)
            {
                return NinId = null;
            }
            else
            {
                feederCodeBase = Convert.ToString(dr[0][col_feedercode]);
            }

            if (!String.IsNullOrEmpty(feederCodeBase))
            {
                int resultcount = Convert.ToInt32(getFeatureCountByTable(substionName, feederName, surveryType, featureType));
                if (resultcount == 0)
                {
                    if (featureType == "pole") { NinId = feederCodeBase + "P0001"; ninIdPrx = "P"; }
                    else if (featureType == "ht_cable") { NinId = feederCodeBase + "C0001"; ninIdPrx = "C"; }
                    else if (featureType == "ht_line") { NinId = feederCodeBase + "L0001"; ninIdPrx = "L"; }
                    else if (featureType == "lt_cable") { NinId = feederCodeBase + "C0001"; ninIdPrx = "LC"; }
                    else if (featureType == "lt_line") { NinId = feederCodeBase + "L0001"; ninIdPrx = "LL"; }
                    else if (featureType == "service_pillar") { NinId = feederCodeBase + "SP001"; ninIdPrx = "SP"; }
                    else if (featureType == "feeder_pillar") { NinId = feederCodeBase + "FP001"; ninIdPrx = "FP"; }
                    else if (featureType == "earth_switch") { NinId = feederCodeBase + "ES001"; ninIdPrx = "ES"; }
                    else if (featureType == "dis_transformer") { NinId = feederCodeBase + "D0001"; ninIdPrx = "D"; }
                    else if (featureType == "rmu") { NinId = feederCodeBase + "R0001"; ninIdPrx = "R"; }
                    else if (featureType == "fuse") { NinId = feederCodeBase + "F0001"; ninIdPrx = "F"; }
                    else if (featureType == "meter") { NinId = feederCodeBase + "M0001"; ninIdPrx = "M"; }
                    else if (featureType == "lig_arrester") { NinId = feederCodeBase + "LA001"; ninIdPrx = "LA"; }
                    else if (featureType == "ct") { NinId = feederCodeBase + "CT001"; ninIdPrx = "CT"; }
                    else if (featureType == "pt") { NinId = feederCodeBase + "PT001"; ninIdPrx = "PT"; }
                    else if (featureType == "dis_box") { NinId = feederCodeBase + "DB001"; ninIdPrx = "DB"; }
                    else if (featureType == "tower_33") { NinId = feederCodeBase + "TW001"; ninIdPrx = "TW"; }
                }
                else
                {

                    if (featureType == "pole") { ninIdPrx = "P"; }
                    else if (featureType == "ht_cable") { ninIdPrx = "C"; }
                    else if (featureType == "ht_line") { ninIdPrx = "L"; }
                    else if (featureType == "lt_cable") { ninIdPrx = "LC"; }
                    else if (featureType == "lt_line") { ninIdPrx = "LL"; }
                    else if (featureType == "service_pillar") { ninIdPrx = "SP"; }
                    else if (featureType == "feeder_pillar") { ninIdPrx = "FP"; }
                    else if (featureType == "earth_switch") { ninIdPrx = "ES"; }
                    else if (featureType == "dis_transformer") { ninIdPrx = "D"; }
                    else if (featureType == "rmu") { ninIdPrx = "R"; }
                    else if (featureType == "fuse") { ninIdPrx = "F"; }
                    else if (featureType == "meter") { ninIdPrx = "M"; }
                    else if (featureType == "lig_arrester") { ninIdPrx = "LA"; }
                    else if (featureType == "ct") { ninIdPrx = "CT"; }
                    else if (featureType == "pt") { ninIdPrx = "PT"; }
                    else if (featureType == "dis_box") { ninIdPrx = "DB"; }
                    else if (featureType == "tower_33") { ninIdPrx = "TW"; }

                    string sql = "select nin_id from " + featureType + "  WHERE fdr_name='"+feederName+"' AND substation_name='"+substionName+"' order by id desc limit 1";

                    using (NpgsqlConnection connection = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["NpgsqlConnectionString"].ConnectionString))
                    {
                        try
                        {
                            NpgsqlCommand cmd = new NpgsqlCommand(sql, connection);
                            connection.Open();
                            NpgsqlDataReader ndr = cmd.ExecuteReader();
                            if (ndr.Read())
                            {
                                string tmp_ninid = (string)ndr["nin_id"];

                                if (ninIdPrx == "LA" || ninIdPrx == "CT" || ninIdPrx == "PT" || ninIdPrx == "DB" || ninIdPrx == "SP" || ninIdPrx == "FP" || ninIdPrx == "PS" || ninIdPrx == "ES" || ninIdPrx == "TW" || ninIdPrx == "LL" || ninIdPrx == "LC")
                                {
                                    int data = Convert.ToInt32(tmp_ninid.Substring(tmp_ninid.Length - 3)) + 1;
                                    string part1 = data.ToString().PadLeft(3, '0');
                                    //int part2 = Convert.ToInt32(NinId.Substring(data, NinId.Length) + 1);
                                    NinId = feederCodeBase + ninIdPrx + part1;
                                }
                                else
                                {
                                    int data = Convert.ToInt32(tmp_ninid.Substring(tmp_ninid.Length - 4)) + 1;
                                    string part1 = data.ToString().PadLeft(4, '0');
                                    //int part2 = Convert.ToInt32(NinId.Substring(data, NinId.Length) + 1);
                                    NinId = feederCodeBase + ninIdPrx + part1;
                                }

                            }
                            connection.Close();
                        }
                        catch (Exception ex)
                        {
                            NinId = null;
                            logger.Error(" Method : GetNiNiD" + ex.Message);
                        }
                    }

                    // string tmp_ninid = "432114101P0001";
                }
            }

            return NinId;
        }






        public List<pole_info> GetPoleList(string substionName, string feederName, string surveryType, string featureType, string dtcname = "NA")
        {
            // select nin_id from pole where  " where substation_name ='" + substionName + "' and feeder_name  ='" + feederName + "' and sur_type ='" + surveryType + "'"

            List<pole_info> poles = null;
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection())
                {
                    string sql = null;
                    if (dtcname == "NA")
                    {
                        sql = "select nin_id,st_x(geom),st_y(geom) from pole where substation_name = '" + substionName + "' and fdr_name = '" + feederName + "' and survey_type = '" + surveryType + "'";
                    }
                    else
                    {
                        sql = "select nin_id,st_x(geom),st_y(geom) from pole where substation_name = '" + substionName + "' and fdr_name = '" + feederName + "' and survey_type = '" + surveryType + "'and dtc_name = '" + dtcname + "'";
                    }

                    connection.ConnectionString = ConfigurationManager.ConnectionStrings["NpgsqlConnectionString"].ToString();
                    connection.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand(sql, connection);
                    NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    connection.Close();
                    if (dt.Rows.Count >= 0)
                    {
                        poles = new List<pole_info>();
                        foreach (DataRow row in dt.Rows)
                        {
                            poles.Add(new pole_info
                            {
                                nin_id = Convert.ToString(row["nin_id"]),
                                x = Convert.ToString(row["st_x"]),
                                y = Convert.ToString(row["st_y"])
                            });
                        }
                    }
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                logger.Error(" Method : GetPoleList" + ex.Message);
            }
            return poles;
        }


        public string getFeatureCountByTable(string substionName, string feederName, string surveryType, string featureType)
        {
            string result = "-1";
            string sql = null;
            if (surveryType == "LT")
            {
                sql = "Select count (*) from " + featureType + " where substation_name ='" + substionName + "' and fdr_name  ='" + feederName + "' and survey_type in('11','LT')";
            }
            else
            {
                sql = "Select count (*) from " + featureType + " where substation_name ='" + substionName + "' and fdr_name  ='" + feederName + "'";
            }



            using (NpgsqlConnection connection = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["NpgsqlConnectionString"].ConnectionString))
            {
                try
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(sql, connection);
                    connection.Open();
                    string count = Convert.ToString(cmd.ExecuteScalar());
                    result = count;
                }
                catch (Exception ex)
                {
                    return result = "-1";
                    logger.Error(" Method : getFeatureCountByTable" + ex.Message);
                }
            }
            return result;
        }


        public string UpdateStatus(string substionName, string feederName, string surveryType, string status)
        {
            string result = "false";
            var sql = String.Empty;
            string updateClm = null;
            string substationclm = null;
            string fdrclm = null;
            string colNameSts = null;
            string colNamefdr = null;
            try
            {
                if (surveryType == "33" || surveryType == "33 KV")
                {
                    updateClm = "status_fdr_33";
                    substationclm = "gss_name_33";
                    fdrclm = "feeder_name_33";
                    colNameSts = "substation_33";
                    colNamefdr = "feeder_33";
                }

                if (surveryType == "11" || surveryType == "11 KV")
                {
                    updateClm = "status_fdr_11";
                    substationclm = "pss_name_11";
                    fdrclm = "feeder_name_11";
                    colNameSts = "substation_11";
                    colNamefdr = "feeder_11";
                }

                //dynamic SRID = 4326; POINT(78.548333 17.417381);
                using (NpgsqlConnection connection = new NpgsqlConnection())
                {
                    sql = "Update master_data set " + updateClm + "='" + status + "' where " + substationclm + "='" + substionName + "' and " + fdrclm + "='" + feederName + "'";

                    connection.ConnectionString = ConfigurationManager.ConnectionStrings["NpgsqlConnectionString"].ToString();
                    connection.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand(sql, connection);
                    int i = cmd.ExecuteNonQuery();
                    int statusid = Convert.ToInt32(DataSet.Tables["lkp_status"].Select("name='" + status + "'")[0]["id"]);

                    sql = "update user_allocation_master set surveystatus=" + statusid + " where " + colNameSts + "='" + substionName + "' AND " + colNamefdr + " ='" + feederName + "'";
                    cmd = new NpgsqlCommand(sql, connection);
                    i = cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    connection.Close();
                    if (i >= 1)
                    {
                        result = "true";
                    }
                    else
                    {
                        result = "false";
                    }
                    //result = true;
                }
            }
            catch (Exception ex)
            {
                result = "false + ex" + ex.Message;
                logger.Error(" Method : UpdateStatus" + ex.Message);
            }
            return result;
        }



        public app_version GetVersionByApp(string app_type)
        {
            app_version app = new app_version();
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection())
                {
                    string sql = "select app_type, version from app_version where app_type = '" + app_type + "'";
                    connection.ConnectionString = ConfigurationManager.ConnectionStrings["NpgsqlConnectionString"].ToString();
                    connection.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand(sql, connection);
                    NpgsqlDataReader data = cmd.ExecuteReader();
                    if (data.HasRows)
                    {
                        while (data.Read())
                        {
                            app.app_type = Convert.ToString(data["app_type"]);
                            app.version = Convert.ToString(data["version"]);
                        }
                    }

                    cmd.Dispose();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                logger.Error(" Method : GetVersionByApp" + ex.Message);
            }
            return app;
        }


        public string validateDate(string dtStr)
        {
            string validDate = "NULL";
            DateTime dd;
            Boolean ch = DateTime.TryParse(dtStr, out dd);
            if (ch)
            {
                string format = "dd-MM-yyyy";
                validDate = "'" + dd.ToString(format) + "'";
            }
            return validDate;
        }



        public List<townDetails> GetTownListDB()
        {

            List<townDetails> towns = new List<townDetails>();

            DataRow[] drs = null;
            try
            {

                DataTable towndt = DataSet.Tables["master_data"].DefaultView.ToTable(true, "town");
                DataTable t1 = DataSet.Tables["master_data"].DefaultView.ToTable(true, "town", "feeder_name_33");
                DataTable t2 = DataSet.Tables["master_data"].DefaultView.ToTable(true, "town", "feeder_name_11");
                DataTable dtc = DataSet.Tables["dis_transformer"].DefaultView.ToTable(true, "town");
                DataTable dt_htline = DataSet.Tables["ht_line"].DefaultView.ToTable(true, "town", "line_len");
                DataTable dt_htcable = DataSet.Tables["ht_cable"].DefaultView.ToTable(true, "town", "line_len");

                foreach (DataRow t in towndt.Rows)
                {
                    string town = (string)t[0];
                    int fdr_33_cnt = 0;
                    int fdr_11_cnt = 0;
                    int dtr_count = 0;
                    double len_ht = 0;
                    DataRow[] dr1 = t1.Select("town = '" + town + "'");
                    fdr_33_cnt = dr1.Count();

                    DataRow[] dr2 = t2.Select("town = '" + town + "'");
                    fdr_11_cnt = dr2.Count();

                    DataRow[] dr3 = dtc.Select("town = '" + town + "'");
                    dtr_count = dr3.Count();

                    DataRow[] drline = dt_htline.Select("town = '" + town + "'");
                    double len1 = 0;
                    foreach (DataRow dr in drline)
                    {
                        len1 += Convert.ToDouble(dr["line_len"]);
                    }

                    DataRow[] drcable = dt_htcable.Select("town = '" + town + "'");
                    //double len1 = 0;
                    foreach (DataRow dr in drcable)
                    {
                        len1 += Convert.ToDouble(dr["line_len"]);
                    }

                    len_ht = len1;


                    townDetails details = new townDetails();
                    details.feedercount_11 = Convert.ToString(fdr_11_cnt);
                    details.feedercount_33 = Convert.ToString(fdr_33_cnt);
                    details.townname = town;
                    details.dtc_count = Convert.ToString(dtr_count);
                    details.ht_length = Convert.ToString(len_ht);
                    details.con_count = Convert.ToString(0);

                    towns.Add(details);

                }


            }
            catch (Exception ex)
            {
                logger.Error(" Method : GetTownListDB" + ex.Message);
            }
            return towns;
        }



        public List<fdrDetails> GetFdrDetailsbyTown(string town)
        {
            List<fdrDetails> frdlist = null;
            string fdrname = null;
            string fdrtyp = null;
            int dtccnt = 0;
            int ht_len = 0;
            int con_cnt = 0;
            try
            {
                frdlist = new List<fdrDetails>();
                DataTable towndt = DataSet.Tables["master_data"].DefaultView.ToTable(true, "town");
                DataRow[] t1 = DataSet.Tables["master_data"].DefaultView.ToTable(true, "town", "feeder_name_33", "status_fdr_33").Select("status_fdr_33<>''");
                DataRow[] t2 = DataSet.Tables["master_data"].DefaultView.ToTable(true, "town", "feeder_name_11", "status_fdr_11").Select("status_fdr_11<>''");

                List<fdrtemp> lst = new List<fdrtemp>();

                foreach (DataRow data in t1)
                {

                    fdrtemp ft = new fdrtemp();
                    ft.town = town;
                    ft.feedername = Convert.ToString(data["feeder_name_33"]);
                    ft.feedertype = "33";
                    ft.status = Convert.ToString(data["status_fdr_33"]);
                    lst.Add(ft);
                }

                foreach (DataRow data in t2)
                {

                    fdrtemp ft = new fdrtemp();
                    ft.town = town;
                    ft.feedername = Convert.ToString(data["feeder_name_11"]);
                    ft.feedertype = "11";
                    ft.status = Convert.ToString(data["status_fdr_11"]);
                    lst.Add(ft);
                }

                foreach (fdrtemp fdrtemp in lst)
                {
                    fdrDetails fdr = new fdrDetails();
                    fdr.feedername = fdrtemp.feedername;
                    fdr.feedertype = fdrtemp.feedertype;
                    DataRow[] dr1 = DataSet.Tables["dis_transformer"].Select("town = '" + town + "' and survey_type='" + fdrtemp.feedertype + "' and fdr_name='" + fdrtemp.feedername + "'");
                    fdr.dtc_count = Convert.ToString(dr1.Count());


                    DataRow[] drline = DataSet.Tables["ht_line"].Select("town = '" + town + "' and survey_type='" + fdrtemp.feedertype + "' and fdr_name='" + fdrtemp.feedername + "'");
                    double len1 = 0;
                    foreach (DataRow dr in drline)
                    {
                        len1 += Convert.ToDouble(dr["line_len"]);
                    }

                    DataRow[] drcable = DataSet.Tables["ht_cable"].Select("town = '" + town + "' and survey_type='" + fdrtemp.feedertype + "' and fdr_name='" + fdrtemp.feedername + "'");
                    //double len1 = 0;
                    foreach (DataRow dr in drcable)
                    {
                        len1 += Convert.ToDouble(dr["line_len"]);
                    }

                    fdr.status = Convert.ToString(fdrtemp.status);
                    fdr.ht_length = Convert.ToString(len1);
                    fdr.con_count = Convert.ToString(0);

                    frdlist.Add(fdr);
                }

            }
            catch (Exception ex)
            {
                logger.Error(" Method : GetFdrDetailsbyTown" + ex.Message);
            }

            return frdlist;
        }



        public List<dtcDetails> GetdtcDetailsbyTownandfdr(string town, string fdrname, string surtyp)
        {
            List<dtcDetails> dtclist = null;
            string dtcname = null;
            string dtcninid = null;
            string lt_len = null;
            string status = null;
            string con_count = null;
            try
            {
                dtclist = new List<dtcDetails>();
                DataRow[] dr1 = DataSet.Tables["dis_transformer"].Select("town = '" + town + "' and survey_type='" + surtyp + "' and fdr_name='" + fdrname + "'");

                dtcDetails dtc = null;

                foreach (DataRow data in dr1)
                {

                    DataRow[] drline = DataSet.Tables["lt_line"].Select("town = '" + town + "' and survey_type='" + surtyp + "' and fdr_name='" + fdrname + "' and dtr_name='" + data["dtr_name"] + "'");
                    double len1 = 0;
                    foreach (DataRow dr in drline)
                    {
                        len1 += Convert.ToDouble(dr["cab_len"]);
                    }

                    DataRow[] drcable = DataSet.Tables["lt_cable"].Select("town = '" + town + "' and survey_type='" + surtyp + "' and fdr_name='" + fdrname + "'and dtr_name='" + data["dtr_name"] + "'");
                    //double len1 = 0;
                    foreach (DataRow dr in drcable)
                    {
                        len1 += Convert.ToDouble(dr["cab_len"]);
                    }

                    dtc = new dtcDetails();
                    dtc.dtcninid = Convert.ToString(data["nin_id"]);
                    dtc.dtcname = Convert.ToString(data["dtr_name"]);
                    dtc.status = Convert.ToString(data["sur_sta_lt"]);
                    dtc.lt_len = Convert.ToString(len1);
                    dtc.con_count = Convert.ToString(0);
                    dtclist.Add(dtc);
                }


            }
            catch (Exception ex)
            {
                logger.Error(" Method : GetdtcDetailsbyTownandfdr" + ex.Message);
            }

            return dtclist;
        }



        public bool createConsumerAndLtLine(consumer consumer)
        {

            bool result = false;
            try
            {
                string ninid = getNinIdByFeature("NA", "NA", "NA", "consumer");
                consumer.nin_id = ninid;
                string sql = "INSERT INTO consumer(nin_id, bld_id, cus_no, cus_name, fat_name, occ_name, tower_no, bld_name, bloc_no, sec_name, loc_name, rural, city_name, pincode, san_load, category, sup_volt, no_of_phase, con_status, met_no, dtr_id, fdr_name, stat_name, pole_id, left_cus_no, met_lo, hig_mount, view_glass, met_type, make_class, seal_no, seal, pole_multi_fdr,substation_name,dtr_name, geom)VALUES('" + consumer.nin_id + "', '" + consumer.bld_id + "','" + consumer.cus_no + "', '" + consumer.fat_name + "', '" + consumer.occ_name + "', '" + consumer.tower_no + "','" + consumer.bld_name + "','" + consumer.bloc_no + "','" + consumer.sec_name + "','" + consumer.loc_name + "', '" + consumer.rural + "', '" + consumer.city_name + "', '" + consumer.pincode + "', '" + consumer.san_load + "', '" + consumer.category + "', '" + consumer.sup_volt + "','" + consumer.no_of_phase + "','" + consumer.con_status + "','" + consumer.met_no + "','" + consumer.dtr_id + "','" + consumer.fdr_name + "','" + consumer.stat_name + "','" + consumer.pole_id + "','" + consumer.left_cus_no + "','" + consumer.met_lo + "','" + consumer.hig_mount + "','" + consumer.view_glass + "','" + consumer.met_type + "','" + consumer.make_class + "','" + consumer.seal_no + "','" + consumer.seal_no + "','" + consumer.seal + "','" + consumer.pole_multi_fdr + "','" + consumer.substation_name + "','" + consumer.dtr_name + "',ST_SetSRID(ST_MakePoint(" + consumer.x + "," + consumer.y + "),4326));";

                string lt_nin_id = getNinIdByFeature("NA", "NA", "C", "lt_line", consumer.dtr_name);
                string x_y = getCoordsOfpole(consumer.pole_id);
                string from_x = x_y.Split(',')[0];
                string from_y = x_y.Split(',')[1];
                string sql1 = "INSERT INTO lt_cable( nin_id, op_volt, fdr_name,  phase, cab_typ, met_no, dtr_name,survey_type, town,create_by, create_date, substation_name, geom,cab_len)VALUES( '" + lt_nin_id + "', 'NA','" + consumer.sup_volt + "', '" + consumer.fdr_name + "', 'Service Cable', '" + consumer.met_no + "', '" + consumer.dtr_name + "', 'C', '" + consumer.city_name + "','" + consumer.createdBy + "', '" + consumer.createdDate+ "','" + consumer.substation_name + "',ST_SetSRID(ST_GeomFromText('LINESTRING(" + from_x + " " + from_y + "," + consumer.x + " " + consumer.y + ")'),4326),ST_Length(ST_Transform(ST_SetSRID(ST_GeomFromText('LINESTRING(" + from_x + " " + from_y + "," + consumer.x + " " + consumer.y + ")'),4326),26986)));";

                using (NpgsqlConnection connection = new NpgsqlConnection())
                {
                    connection.ConnectionString = ConfigurationManager.ConnectionStrings["NpgsqlConnectionString"].ToString();
                    connection.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand(sql, connection);
                    int i = cmd.ExecuteNonQuery();
                    cmd = new NpgsqlCommand(sql1, connection);
                    i = cmd.ExecuteNonQuery();
                    if (i >= 1)
                    {
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }


                    cmd.Dispose();
                    connection.Close();
                }

            }
            catch (Exception ex)
            {
                result = false;
                logger.Error("Method : createConsumer: " + ex.Message);
            }
            return result;

        }

        public bool createBuilding(Building building)
        {
            bool result = false;
            try
            {
                string ninid = getNinIdByFeature("NA", "NA", "NA", "building");
                building.nin_id = ninid;
                using (NpgsqlConnection connection = new NpgsqlConnection())
                {
                    //string geomstring = "84.80647087097167,19.289311976076515|84.80792999267578,19.287286654151362|84.81204986572264,  19.2889879262522|84.81050491333008,19.29162081250786| 84.80647087097167,19.289311976076515";
                    string geomstring = building.buildingGeomString;

                    string b1 = geomstring.Replace(',', ' ');
                    string c1 = b1.Replace('|', ',');

                    string geom = "ST_SetSRID(ST_GeomFromText('POLYGON((" + c1 + "))'),4326)";
                    //geomstring.Replace(',', ' ').Replace('|', ',');

                    //building(
                    var sql = "INSERT INTO building(nin_id, substaion_name, bld_name, address, bld_type, bld_num, bld_r_num, city, con_status, dist, no_of_conn, no_if_flat, no_of_floor, no_of_shop, pincode, soc_name, sub_loc, created_by,created_date,geom) VALUES( '" + building.nin_id + "','" + building.substaion_name + "','" + building.bld_name + "','" + building.address + "','" + building.bld_type + "','" + building.bld_num + "','" + building.bld_r_num + "', '" + building.city + "', '" + building.con_status + "','" + building.dist + "','" + building.no_of_conn + "','" + building.no_if_flat + "','" + building.no_of_floor + "', '" + building.no_of_shop + "','" + building.pincode + "','" + building.soc_name + "', '" + building.sub_loc + "', '" + building.createdBy + "', '" + building.createdDate + "'," + geom + ");";


                    //var sql = "INSERT INTO building(nin_id, bld_name, bld_type, town, substation_name, geom)VALUES('" + building.nin_id + "', '" + building.bld_name + "', '" + building.bld_type + "', '" + building.city + "', '" + building.substaion_name + "', " + geom + "); ";
                    connection.ConnectionString = ConfigurationManager.ConnectionStrings["NpgsqlConnectionString"].ToString();
                    connection.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand(sql, connection);
                    int i = cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    connection.Close();
                    if (i >= 1)
                    {
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                    //result = true;
                }
            }
            catch (Exception ex)
            {
                result = false;
                logger.Error(" Method : GetdtcDetailsbyTownandfdr" + ex.Message);
            }
            return result;
        }






        public Dictionary<string, string> GetDashboardData(string townname = "NA")
        {
            Dictionary<string, string> valuePairs = null;
            try
            {
                string dis_transformer = "0";
                string town = "0";
                string user_master = "0";
                string consumer = "000";
                string fuse = "0";
                string pole = "0";
                string sub_station_33 = "0";
                string sub_station_11 = "0";
                string feeder_33 = "0";
                string feeder_11 = "0";
                valuePairs = new Dictionary<string, string>();
                town = Convert.ToString(DataSet.Tables["town"].Rows.Count);
                user_master = Convert.ToString(DataSet.Tables["user_master"].Rows.Count);
                if (townname == "NA")
                {
                    dis_transformer = Convert.ToString(DataSet.Tables["dis_transformer"].Rows.Count);
                    fuse = Convert.ToString(DataSet.Tables["fuse"].Rows.Count);
                    pole = Convert.ToString(DataSet.Tables["pole"].Rows.Count);
                    DataTable data_s_33 = DataSet.Tables["master_data"].DefaultView.ToTable(true, "gss_name_33");
                    sub_station_33 = Convert.ToString(data_s_33.Rows.Count);
                    DataTable data_s_11 = DataSet.Tables["master_data"].DefaultView.ToTable(true, "pss_name_11");
                    sub_station_11 = Convert.ToString(data_s_11.Rows.Count);
                    DataTable data_f_33 = DataSet.Tables["master_data"].DefaultView.ToTable(true, "feeder_name_33");
                    feeder_33 = Convert.ToString(data_f_33.Rows.Count);
                    DataTable data_f_11 = DataSet.Tables["master_data"].DefaultView.ToTable(true, "feeder_name_11");
                    feeder_11 = Convert.ToString(data_f_11.Rows.Count);
                }
                else
                {

                    dis_transformer = Convert.ToString(DataSet.Tables["dis_transformer"].Select("town='" + townname + "'").Count());
                    fuse = Convert.ToString(DataSet.Tables["fuse"].Select("town='" + townname + "'").Count());
                    pole = Convert.ToString(DataSet.Tables["pole"].Select("town='" + townname + "'").Count());
                    DataTable data_s_33 = DataSet.Tables["master_data"].DefaultView.ToTable(true, "town", "gss_name_33");
                    sub_station_33 = Convert.ToString(data_s_33.Select("town='" + townname + "'").Count());
                    DataTable data_s_11 = DataSet.Tables["master_data"].DefaultView.ToTable(true, "town", "pss_name_11");
                    sub_station_11 = Convert.ToString(data_s_11.Select("town='" + townname + "'").Count());
                    DataTable data_f_33 = DataSet.Tables["master_data"].DefaultView.ToTable(true, "town", "feeder_name_33");
                    feeder_33 = Convert.ToString(data_f_33.Select("town='" + townname + "'").Count());
                    DataTable data_f_11 = DataSet.Tables["master_data"].DefaultView.ToTable(true, "town", "feeder_name_11");
                    feeder_11 = Convert.ToString(data_f_11.Select("town='" + townname + "'").Count());
                }

                valuePairs.Add("dis_transformer", dis_transformer);
                valuePairs.Add("town", town);
                valuePairs.Add("user_master", user_master);
                valuePairs.Add("consumer", consumer);
                valuePairs.Add("fuse", fuse);
                valuePairs.Add("pole", pole);
                valuePairs.Add("sub_station_33", sub_station_33);
                valuePairs.Add("sub_station_11", sub_station_11);
                valuePairs.Add("feeder_33", feeder_33);
                valuePairs.Add("feeder_11", feeder_11);
            }
            catch (Exception ex)
            {
                logger.Error(" Method : GetDashboardData" + ex.Message);
            }
            return valuePairs;
        }


        //get extent by table select st_extent(geom) from ht_line pram {town,substation,distribution,feeder}

        public string GetExtentByTown(string townname = "NA", string fdrName = "NA")
        {
            string result = null;

            string sql = null;
            if (townname == "NA")
            {
                sql = "select st_AsText(st_extent(geom)) as extent from pole";
            }
            else if (townname != "NA" && fdrName == "NA")
            {
                sql = "select st_AsText(st_extent(geom)) as extent from pole where town='" + townname + "'";
            }
            else if (townname != "NA" && fdrName != "NA")
            {
                sql = "select st_AsText(st_extent(geom)) as extent from pole where town='" + townname + "' AND fdr_name='" + fdrName + "'";
            }
            else
            {
                //For time being I have changed later we have to put condition for DTR
                sql = "select st_AsText(st_extent(geom)) as extent from pole";
            }

            using (NpgsqlConnection connection = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["NpgsqlConnectionString"].ConnectionString))
            {
                try
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(sql, connection);
                    connection.Open();
                    NpgsqlDataReader data = cmd.ExecuteReader();
                    if (data.HasRows)
                    {
                        while (data.Read())
                        {
                            string temp = Convert.ToString(data["extent"]);
                            string myString = temp.Substring(9);
                            result = myString.Remove(myString.Length - 2).Split(',')[0] + "," + myString.Remove(myString.Length - 2).Split(',')[2];
                        }
                    }
                    connection.Close();
                    cmd.Dispose();
                    //result = count;
                }
                catch (Exception ex)
                {
                    logger.Error(" Method : GetExtentByTown" + ex.Message);
                    result = ex.Message;
                }
            }
            return result;
        }


        public string GetBuildingIdByConsumer(string x, string y)
        {
            string NinId = null;
            try
            {
                string sql = "select * from building  where ST_Contains(geom,st_setsrid(st_MakePoint("+x+","+y+"),4326))";
                using (NpgsqlConnection connection = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["NpgsqlConnectionString"].ConnectionString))
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(sql, connection);
                    connection.Open();
                    NpgsqlDataReader data = cmd.ExecuteReader();
                    if (data.HasRows)
                    {
                        while (data.Read())
                        {
                            NinId = Convert.ToString(data["nin_id"]);
                        }
                    }
                    connection.Close();
                    cmd.Dispose();
                }

            }
            catch (Exception ex)
            {
                logger.Error("Method : GetBuildingIdByConsumer" + ex.Message);
            }
            return NinId;
        }




        public List<dtr_master> GetDtrList(string substionName, string feederName)
        {
           
            List<dtr_master> dtr_s = null;
            List<dtr_master> dtr_m = null;
            try
            {
                dtr_s = new List<dtr_master>();
                DataRow[] datas = DataSet.Tables["dtr_master"].Select("fdr_name_11='" + feederName + "' AND pss_name='" + substionName + "' AND is_capture='N'");
                dtr_master dtr_ = null;
                foreach (DataRow dr in datas) {
                    dtr_ = new dtr_master();
                    dtr_.dtc_name = Convert.ToString(dr["dtc_name"]);
                    dtr_.capacity = Convert.ToString(dr["capacity"]);
                    dtr_s.Add(dtr_);
                }
                 dtr_m = dtr_s.OrderBy(x => x.dtc_name).ToList();
            }
            catch (Exception ex)
            {
                logger.Error(" Method : GetDtrList : " + ex.Message);
            }
            return dtr_m;
        }


        public bool UpdateDtrStatus(string feederName, string substationName, string dtcName) {
            bool result = false;
            try
            {
               string sql = "UPDATE dtr_master  SET  is_capture ='Y' WHERE pss_name='" + substationName + "'AND fdr_name_11='"+feederName+"' AND dtc_name='" +dtcName + "' ; ";

                using (NpgsqlConnection connection = new NpgsqlConnection())
                {
                    connection.ConnectionString = ConfigurationManager.ConnectionStrings["NpgsqlConnectionString"].ToString();
                    connection.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand(sql, connection);
                    int i = cmd.ExecuteNonQuery();

                    cmd = new NpgsqlCommand(sql, connection);
                    i = cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    connection.Close();
                    if (i >= 1)
                    {
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }

                }

            }
            catch (Exception ex) {
                result = false;
                logger.Error("Method : UpdateDtrStatus : ", ex.Message);
            }
            return result;
        }

    }
}