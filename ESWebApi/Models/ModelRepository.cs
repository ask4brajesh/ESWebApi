using Newtonsoft.Json;
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

        public DataTable DataTableMaster = null;
        public DataSet DataSet = null;

        public ModelRepository()
        {
            using (NpgsqlConnection connection = new NpgsqlConnection())
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

                cmd = new NpgsqlCommand("select nin_id from lt_line", connection);
                adapter = new NpgsqlDataAdapter(cmd);
                adapter.Fill(DataSet, "lt_line");

                cmd = new NpgsqlCommand("select nin_id from lt_cable", connection);
                adapter = new NpgsqlDataAdapter(cmd);
                adapter.Fill(DataSet, "lt_cable");

                //adapter.Fill(DataTableMaster);
                connection.Close();
            }

        }
        public List<User> GetUserList()
        {
            List<User> user = new List<User>();
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection())
                {
                    string sql = "select * from user_master";
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
                        user.Add(
                            new User
                            {
                                userid = Convert.ToInt32(row["userid"]),
                                username = Convert.ToString(row["name"]),
                                usertypeid = Convert.ToInt32(row["usertypeid"]),
                                emailid = Convert.ToString(row["emailid"]),
                                mobileno = Convert.ToString(row["mobileno"]),
                                adharcardno = Convert.ToString(row["adharcardno"]),
                                isactive = Convert.ToBoolean(row["isactive"])
                                // create_date = Convert.ToDateTime(row["create_date"])
                            });
                    }
                    cmd.Dispose();
                    connection.Close();

                }
            }
            catch (Exception ex) { }
            return user;
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
            catch (Exception ex) { }
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
            catch (Exception ex) { }
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
                    if (surveytype == "33")
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
            catch (Exception ex) { }
            return st;
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

                    if (fdrtype == "33")
                    {
                        fldFdr_name = "feeder_name_33";
                        fldFdr_code = "feeder_code_33";
                        fldss_name = "gss_name_33";
                    }

                    if (fdrtype == "11")
                    {
                        fldFdr_name = "feeder_name_11";
                        fldFdr_code = "feeder_code_11";
                        fldss_name = "pss_name_11";
                    }


                    string sql = "select " + fldFdr_name + ", " + fldFdr_code + " from master_data where " + fldss_name + " = '" + substation + "';";
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
                                code = Convert.ToString(row[fldFdr_code])
                            });
                    }
                    cmd.Dispose();
                    connection.Close();
                }
            }
            catch (Exception ex) { }
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
                    string sql = "select User_Allocation_master.* from User_Allocation_master inner join user_master on User_Allocation_master.userid = user_master.userid where lower(user_master.name) ='" + userid.ToLower().Trim() + "'";
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
                                dtr_nin =Convert.ToString(row["dtr_nin"])
                            });
                    }
                    cmd.Dispose();
                    connection.Close();
                }
            }
            catch (Exception ex) { }
            return assigns;
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
            catch (Exception ex) { }
            return feedercode;
        }


        public List<featuregeojson> GetGeojsonAsync(string substaionname, string feedername, string surveytype, string town,string dt_name)
        {
            var requestUrl = String.Empty;
            dynamic result = null;
            string[] features = null;
            

            List<featuregeojson> feature = new List<featuregeojson>();
            string cql_filter = null;
            if (surveytype == "LT") {
               features =  new string[]{
              "dis_transformer","lt_cable","lt_line"
            };
                cql_filter = "town='"+town+"' AND fdr_name='" + feedername + "' AND substation_name='" + substaionname + "' AND survey_type = '" + surveytype + "'AND dtr_name='"+ dt_name + "'";
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
              "dis_transformer","lt_cable","lt_line","pole" };
                cql_filter = "town='" + town + "' AND fdr_name = '" + feedername + "' AND substation_name='" + substaionname + "' AND dtr_name='"+dt_name+"'";
            }


            for (int i = 0; i < features.Length; i++)
            {
                try
                {
                    result = null;
                    //requestUrl = "http://access.spaceimagingme.com:6093/geoserver/cite/ows?service=WFS&version=1.0.0&request=GetFeature&typeName=cite:" + features[i] + "&maxFeatures=100&CQL_filter=fdr_name='" + feedername + "' and  substation_name='" + substaionname + "' and survey_type='" + surveytype + "'&outputFormat=application/json";

                    requestUrl = "http://access.spaceimagingme.com:6093/geoserver/cite/wfs?service=wfs&version=2.0.0&request=GetFeature&typeNames=cite:" + features[i] + "&propertyName=nin_id,geom&cql_filter="+ cql_filter + "&outputFormat=application/json";
                    var req = WebRequest.Create(requestUrl);
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
                string sql = "select st_x(geom),st_y(geom) from dis_transformer where nin_id='"+ dtrnin + "'";
                using (NpgsqlConnection connection = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["NpgsqlConnectionString"].ConnectionString))
                {
                    try
                    {
                        NpgsqlCommand cmd = new NpgsqlCommand(sql, connection);
                        connection.Open();
                        NpgsqlDataReader ndr = cmd.ExecuteReader();
                        if (ndr.HasRows) {
                            if (ndr.Read())
                            {
                                NinId = Convert.ToString(ndr[0])+","+Convert.ToString(ndr[1]);
                            }
                        }

                       
                        connection.Close();
                        connection.Dispose();
                    }
                    catch (Exception ex)
                    {
                        NinId = null;
                    }
                }

            }
            catch (Exception ex) {
                NinId = ex.Message;
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
                    // NinId = null;
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
            catch (Exception ex) { }
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
                sql1 = string.Format("Insert into pole (nin_id,fdr_name,sec_name,pre_pol_no,stud_pole,earthing,oth_cable,status,cut_point,op_volt,construct,po_assem,type,net_arran,rising,sec_ins,num_of_ins,type_of_ins,no_stay,trans_ins,crs_arm,st_light,sw_mt_ins,tp_str_lig,st_fit_wat,st_fit_typ,li_arr_ins,sur_remark,create_by,create_date,substation_name,survey_type,town,con_to,dtc_name,geom)values('" + data1.nin_id + "', '" + data1.fdr_name + "', '" + data1.sec_name + "','" + data1.pre_pol_no + "', '" + data1.stud_pole + "', '" + data1.earthing + "', '" + data1.oth_cable + "','" + data1.status + "','" + data1.cut_point + "','" + data1.op_volt + "','" + data1.construct + "','" + data1.po_assem + "','" + data1.type + "','" + data1.net_arran + "','" + data1.rising + "','" + data1.sec_ins + "','" + data1.num_of_ins + "','" + data1.type_of_ins + "','" + data1.no_stay + "','" + data1.trans_ins + "','" + data1.crs_arm + "','" + data1.st_light + "','" + data1.sw_mt_ins + "','" + data1.tp_str_lig + "','" + data1.st_fit_wat + "','" + data1.st_fit_typ + "','" + data1.li_arr_ins + "','" + data1.sur_remark + "','" + data1.create_By + "','" + data1.create_Date + "','" + data1.substation_name + "','" + data1.survey_type + "','" + data1.town + "','" + data1.con_to + "','" + data1.dtc_name + "',ST_SetSRID(ST_MakePoint(" + data1.x + "," + data1.y + "),4326));");
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
                ninid = data1.nin_id;
                sql1 = string.Format("INSERT INTO public.dis_transformer( nin_id, con_to_pol, earthing, ear_type, dtr_code, st_name, status, man_name, man_s_no, man_date, dtr_name, fencing, type, sub_type, usage, capacity, hi_v_si_v, lo_v_si_v, cor_typ, vec_grp, per_imp, lv_fdr_pr, hv_pro, phase, lv_pro, own_by, no_of_lt_p, no_oflt_fd, met_no, mod_num, pre_of_tap, cur_tap_po, tap_ch_typ, min_tap, max_tap, tp_ch_m_nm, tp_ch_s_no, tr_nm_phot, geom, sur_remark, create_by, create_date, substation_name, survey_type, sec_name, town,fdr_name) VALUES('" + data1.nin_id + "','" + data1.con_to_pol + "','" + data1.earthing + "', '" + data1.ear_type + "', '" + data1.dtr_code + "', '" + data1.st_name + "', '" + data1.status + "', '" + data1.man_name + "', '" + data1.man_s_no + "', " + validateDate(data1.man_date) + ", '" + data1.dt_name + "', '" + data1.fencing + "', '" + data1.type + "', '" + data1.sub_type + "', '" + data1.usage + "', '" + data1.capacity + "', '" + data1.hi_v_si_v + "', '" + data1.lo_v_si_v + "', '" + data1.cor_typ + "', '" + data1.vec_grp + "', '" + data1.per_imp + "', '" + data1.lv_fdr_pr + "', '" + data1.hv_pro + "', '" + data1.phase + "', '" + data1.lv_pro + "', '" + data1.own_by + "', '" + data1.no_of_lt_p + "', '" + data1.no_oflt_fd + "', '" + data1.met_no + "', '" + data1.mod_num + "', '" + data1.pre_of_tap + "', '" + data1.cur_tap_po + "', '" + data1.tap_ch_typ + "', '" + data1.min_tap + "', '" + data1.max_tap + "', '" + data1.tp_ch_m_nm + "', '" + data1.tp_ch_s_no + "', '" + data1.tr_nm_phot + "',ST_SetSRID(ST_MakePoint(" + data1.x + "," + data1.y + "),4326), '" + data1.sur_remark + "', '" + data1.create_by + "', '" + data1.create_date + "', '" + data1.substation_name + "', '" + data1.survey_type + "', '" + data1.sec_name + "','" + data1.town + "', '" + data1.fdr_name + "');");
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
                result = "false + ex" + ex.Message;
            }
            return result;
        }

        public string getNinIdByFeature(string substionName, string feederName, string surveryType, string featureType,string dtcname = "NA", string con_to = "NA", string curr_pole = "NA")
        {
            DataRow[] dr = null;
            string feederCodeBase = null;
            string col_feedercode = null;
            string NinId = null;
            string ninIdPrx = null;
            string circuit = null;
            if (dtcname != "NA" && surveryType == "LT")
            {
                if (featureType == "lt_line" || featureType == "lt_cable") {
                    DataRow[] rows = DataSet.Tables["dis_transformer"].Select("dtr_name='" + dtcname + "'");
                    string dtrid = Convert.ToString(rows[0]["nin_id"]);
                    string cpole = curr_pole;
                    string dtid = cpole.Substring(0, cpole.Length - 4);
                    DataRow[] prows = DataSet.Tables[featureType].Select("nin_id LIKE '%" + dtid + "%'");
                    if (prows.Length == 0) {
                        if (featureType == "lt_line")
                        {
                            NinId = dtid + "L001";
                        }

                        if (featureType == "lt_cable")
                        {
                            NinId = dtid + "C001";
                        }
                    }
                    else {
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

                if (featureType == "pole") {
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

                if (featureType == "service_pillar" || featureType == "fuse" || featureType == "feeder_pillar") {

                    dr = DataSet.Tables["master_data"].Select("feeder_name_11 = '" + feederName + "' and pss_name_11 = '" + substionName + "'");
                    col_feedercode = "feeder_code_11";

                    if (dr.Length == 0)
                    {
                        return NinId = null;
                    }
                    else { 
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
           

            if (featureType == "building") {
                int cntbld = DataSet.Tables["building"].Rows.Count;
                if (cntbld == 0)
                {
                    NinId = "BLD0000001";
                }
                else {
                    DataRow dr1 = DataSet.Tables["building"].Rows[cntbld-1];
                    string tmp_ninid = Convert.ToString(dr1["nin_id"]);
                    int data = Convert.ToInt32(tmp_ninid.Substring(tmp_ninid.Length - 7)) + 1;
                    string part1 = data.ToString().PadLeft(7, '0');
                    NinId = "BLD" + part1;
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

                    string sql = "select nin_id from " + featureType +" where survey_type='"+surveryType+"' order by id desc limit 1";

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






        public List<pole_info> GetPoleList(string substionName, string feederName, string surveryType, string featureType,string dtcname = "NA")
        {
            // select nin_id from pole where  " where substation_name ='" + substionName + "' and feeder_name  ='" + feederName + "' and sur_type ='" + surveryType + "'"

            List<pole_info> poles = null;
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection())
                {
                    string sql = null;
                    if (dtcname == "NA") {
                        sql = "select nin_id,st_x(geom),st_y(geom) from pole where substation_name = '" + substionName + "' and fdr_name = '" + feederName + "' and survey_type = '" + surveryType + "'";
                    }
                    else {
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
            else {
                sql = "Select count (*) from " + featureType + " where substation_name ='" + substionName + "' and fdr_name  ='" + feederName + "' and survey_type ='" + surveryType + "'";
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

            try
            {
                if (surveryType == "33")
                {
                    updateClm = "status_fdr_33";
                    substationclm = "gss_name_33";
                    fdrclm = "fdr_name_33";
                }

                if (surveryType == "11")
                {
                    updateClm = "status_fdr_11";
                    substationclm = "pss_name_11";
                    fdrclm = "fdr_name_11";
                }

                //dynamic SRID = 4326; POINT(78.548333 17.417381);
                using (NpgsqlConnection connection = new NpgsqlConnection())
                {
                    sql = "Update master_data set " + updateClm + "='" + status + "' where " + substationclm + "='" + substionName + "' and " + fdrclm + "='" + feederName + "'";

                    connection.ConnectionString = ConfigurationManager.ConnectionStrings["NpgsqlConnectionString"].ToString();
                    connection.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand(sql, connection);
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
                    //result = true;
                }
            }
            catch (Exception ex)
            {
                result = "false + ex" + ex.Message;
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
            catch (Exception ex) { }
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
            catch (Exception ex) { }
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
                DataRow[] t1 = DataSet.Tables["master_data"].DefaultView.ToTable(true, "town","feeder_name_33","status_fdr_33").Select("status_fdr_33<>''");
                DataRow[] t2 = DataSet.Tables["master_data"].DefaultView.ToTable(true, "town", "feeder_name_11", "status_fdr_11").Select("status_fdr_11<>''");

                List<fdrtemp> lst = new List<fdrtemp>();

                foreach (DataRow data in t1)
                {

                    fdrtemp ft = new fdrtemp();
                    ft.town = town;
                    ft.feedername = Convert.ToString(data["feeder_name_33"]);
                    ft.feedertype = "33";
                    lst.Add(ft);
                }

                foreach (DataRow data in t2)
                {

                    fdrtemp ft = new fdrtemp();
                    ft.town = town;
                    ft.feedername = Convert.ToString(data["feeder_name_11"]);
                    ft.feedertype = "11";
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

                    fdr.ht_length = Convert.ToString(len1);
                    fdr.con_count = Convert.ToString(0);

                    frdlist.Add(fdr);
                }

            }
            catch (Exception ex)
            {


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
                    dtc = new dtcDetails();
                    dtc.dtcninid = Convert.ToString(data["nin_id"]);
                    dtc.dtcname = Convert.ToString(data["dt_name"]);
                    dtc.status  = Convert.ToString(data["sur_sta_lt"]);

                    dtclist.Add(dtc);
                }

             
            }
            catch (Exception ex)
            {


            }

            return dtclist;
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

                    var sql = "INSERT INTO building(nin_id, bld_name, bld_type, town, substation_name, geom)VALUES('" + building.nin_id + "', '" + building.buildingName + "', '" + building.buildingType + "', '" + building.town + "', '" + building.substaion_name + "', " + geom + "); ";
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

            }
            return valuePairs;
        }


        //get extent by table select st_extent(geom) from ht_line pram {town,substation,distribution,feeder}

        public string GetExtentByTown(string townname = "NA")
        {
            string result = null;

            string sql = null;
            if (townname == "NA")
            {

                sql = "select st_AsText(st_extent(geom)) as extent from pole";
            }
            else
            {
                sql = "select st_AsText(st_extent(geom)) as extent from pole where town='" + townname + "'";
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
                    result = ex.Message;
                }
            }
            return result;
        }





    }
}