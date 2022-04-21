using ESWebApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;

namespace ESWebApi.Controllers
{
    [AllowAnonymous]
    public class ValuesController : ApiController
    {
        ModelRepository model = new ModelRepository();



        [HttpGet]
        [Route("api/msit/getuserlist")]
        public IHttpActionResult GetUserList()
        {
            var identity = (ClaimsIdentity)User.Identity;
            dynamic user = model.GetUserList();
            ResponceData<dynamic> responce = new ResponceData<dynamic>();
            if (user.Count > 0)
            {
                responce.Data = user;
                responce.message = "success";
                responce.status = ResultCodeType.SUCCESS;
            }
            else
            {
                responce.message = "data not found";
                responce.status = ResultCodeType.WARNING;
            }

            return Ok(responce);
        }


        [HttpPost]
        [Route("api/msit/createuser")]
        public IHttpActionResult CreateUser(CreateUser user)
        {
            var identity = (ClaimsIdentity)User.Identity;
            bool result = model.createuser(user);
            ResponceData<dynamic> responce = new ResponceData<dynamic>();
            if (result == true)
            {
                responce.Data = user;
                responce.message = "success";
                responce.status = ResultCodeType.SUCCESS;
            }
            else
            {
                responce.message = "failed";
                responce.status = ResultCodeType.FAIL;
            }
            return Ok(responce);
        }


        [HttpGet]
        [Route("api/msit/gettownlist")]
        public IHttpActionResult GetTownList()
        {
            var identity = (ClaimsIdentity)User.Identity;
            List<Town> towns = model.GetTownList();
            ResponceData<dynamic> responce = new ResponceData<dynamic>();
            if (towns.Count() > 0)
            {
                responce.Data = towns;
                responce.message = "success";
                responce.status = ResultCodeType.SUCCESS;
            }
            else
            {
                responce.message = "failed";
                responce.status = ResultCodeType.FAIL;
            }
            return Ok(responce);
        }


        [HttpGet]
        [Route("api/msit/getsurveytype")]
        public IHttpActionResult GetSurveyType()
        {
            var identity = (ClaimsIdentity)User.Identity;
            List<SurveyType> surveyTypes = model.GetSurveyTypeList();
            ResponceData<dynamic> responce = new ResponceData<dynamic>();
            if (surveyTypes.Count() > 0)
            {
                responce.Data = surveyTypes;
                responce.message = "success";
                responce.status = ResultCodeType.SUCCESS;
            }
            else
            {
                responce.message = "failed";
                responce.status = ResultCodeType.FAIL;
            }
            return Ok(responce);
        }



        [Route("api/msit/getsubstationbytown")]
        public IHttpActionResult GetSubStationbyTown(string town, string surveytype)
        {
            var identity = (ClaimsIdentity)User.Identity;
            List<SubStation> subStations = model.GetSubStaionList(town, surveytype);

            ResponceData<dynamic> responce = new ResponceData<dynamic>();
            if (subStations.Count() > 0)
            {
                responce.Data = subStations;
                responce.message = "success";
                responce.status = ResultCodeType.SUCCESS;
            }
            else
            {
                responce.message = "failed";
                responce.status = ResultCodeType.FAIL;
            }
            return Ok(responce);
        }


        [HttpGet]
        [Route("api/msit/getfeederbysubstation")]
        public IHttpActionResult GetFeederBySubStation(string substation, string fdrtype)
        {
            var identity = (ClaimsIdentity)User.Identity;
            List<FeederName> feeders = model.GetFeederList(substation, fdrtype);
            ResponceData<dynamic> responce = new ResponceData<dynamic>();
            if (feeders.Count() > 0)
            {
                responce.Data = feeders;
                responce.message = "success";
                responce.status = ResultCodeType.SUCCESS;
            }
            else
            {
                responce.message = "failed";
                responce.status = ResultCodeType.FAIL;
            }
            return Ok(responce);
        }


        [Route("api/msit/getassigntaskbyuserid")]
        public IHttpActionResult GetassigntaskbyUserid(string username)
        {
            var identity = (ClaimsIdentity)User.Identity;
            List<AssignTask> assigns = model.GetAssignTaskListbyid(username);
            ResponceData<dynamic> responce = new ResponceData<dynamic>();
            if (assigns.Count() > 0)
            {
                responce.Data = assigns;
                responce.message = "success";
                responce.status = ResultCodeType.SUCCESS;
            }
            else
            {
                responce.message = "failed";
                responce.status = ResultCodeType.FAIL;
            }
            return Ok(responce);
        }


        [Route("api/msit/getgeojson")]
        public IHttpActionResult Getgeojson(string substaionname, string feedername, string surveytype, string town, string dt_name)
        {
            var identity = (ClaimsIdentity)User.Identity;
            var geojson = model.GetGeojsonAsync(substaionname, feedername, surveytype, town, dt_name);
            ResponceData<dynamic> responce = new ResponceData<dynamic>();
            if (geojson != null)
            {
                responce.Data = geojson;
                responce.message = "success";
                responce.status = ResultCodeType.SUCCESS;
            }
            else
            {
                responce.message = "failed";
                responce.status = ResultCodeType.FAIL;
            }
            return Ok(responce);
        }


        [Route("api/msit/getsectionlist")]
        public IHttpActionResult GetSectionList()
        {
            // var list = model.GetFdrDetailsbyTown("BALASORE");
            var identity = (ClaimsIdentity)User.Identity;
            var slist = model.GetSectionList();
            ResponceData<dynamic> responce = new ResponceData<dynamic>();
            if (slist != null)
            {
                responce.Data = slist;
                responce.message = "success";
                responce.status = ResultCodeType.SUCCESS;
            }
            else
            {
                responce.message = "failed";
                responce.status = ResultCodeType.FAIL;
            }
            return Ok(responce);
        }


        //[Route("api/msit/syncpolelist")]
        //public IHttpActionResult SyncDataList( string jsondata, string table)
        //{
        //    var identity = (ClaimsIdentity)User.Identity;
        //    var slist = model.SaveJsonlist(jsondata);
        //    ResponceData<dynamic> responce = new ResponceData<dynamic>();
        //    if (slist == true)
        //    {
        //        responce.Data = slist;
        //        responce.message = "success";
        //        responce.status = ResultCodeType.SUCCESS;
        //    }
        //    else
        //    {
        //        responce.message = "failed";
        //        responce.status = ResultCodeType.FAIL;
        //    }
        //    return Ok(responce);
        //}


        [Route("api/msit/getNinIdByFeature")]
        public IHttpActionResult GetNinIdByFeature(string substionName, string feederName, string surveryType, string featureType, string dtcname = "NA", string con_to = "NA", string curr_pole = "NA")
        {


            var identity = (ClaimsIdentity)User.Identity;
            var slist = model.getNinIdByFeature(substionName, feederName, surveryType, featureType, dtcname, con_to, curr_pole);
            ResponceData<dynamic> responce = new ResponceData<dynamic>();
            if (slist != null)
            {
                responce.Data = slist;
                responce.message = "success";
                responce.status = ResultCodeType.SUCCESS;
            }
            else
            {
                responce.message = "failed";
                responce.status = ResultCodeType.FAIL;
            }
            return Ok(responce);
        }





        [Route("api/msit/GetPoleList")]
        public IHttpActionResult GetPoleList(string substionName, string feederName, string surveryType, string featureType, string dtcname = "NA")
        {
            var identity = (ClaimsIdentity)User.Identity;
            var slist = model.GetPoleList(substionName, feederName, surveryType, featureType, dtcname);
            ResponceData<dynamic> responce = new ResponceData<dynamic>();
            if (slist != null)
            {
                if (slist.Count() >= 0)
                {
                    responce.Data = slist;
                    responce.message = "success";
                    responce.status = ResultCodeType.SUCCESS;
                }
            }
            else
            {
                responce.message = "failed";
                responce.status = ResultCodeType.FAIL;
            }
            return Ok(responce);
        }

        [HttpGet]
        [Route("api/msit/updatestatus")]
        public IHttpActionResult UpdateStatus(string substionName, string feederName, string surveryType, string status)
        {
            var identity = (ClaimsIdentity)User.Identity;
            var slist = model.UpdateStatus(substionName, feederName, surveryType, status);
            ResponceData<dynamic> responce = new ResponceData<dynamic>();
            if (slist == "true")
            {
                responce.Data = slist;
                responce.message = "success";
                responce.status = ResultCodeType.SUCCESS;
            }
            else
            {
                responce.message = "failed";
                responce.status = ResultCodeType.FAIL;
            }
            return Ok(responce);
        }


        [Route("api/msit/createfeature")]
        [HttpPost]
        public IHttpActionResult Createfeature(FeatureType feature)
        {
            var identity = (ClaimsIdentity)User.Identity;
            string data = Convert.ToString(feature.data);
            string featuretable = feature.featuretable;
            var sa = model.SaveJsonlist(data, featuretable);
            ResponceData<dynamic> responce = new ResponceData<dynamic>();
            if (sa == "true")
            {
                responce.Data = sa;
                responce.message = "success";
                responce.status = ResultCodeType.SUCCESS;
            }
            else
            {
                responce.message = sa;
                responce.status = ResultCodeType.FAIL;
            }
            return Ok(responce);
        }

        [Route("api/msit/getcurrentversion")]
        public IHttpActionResult GetCurrentVersion(string app_type)
        {
            var identity = (ClaimsIdentity)User.Identity;
            var slist = model.GetVersionByApp(app_type);
            ResponceData<dynamic> responce = new ResponceData<dynamic>();
            if (slist != null)
            {
                responce.Data = slist;
                responce.message = "success";
                responce.status = ResultCodeType.SUCCESS;
            }
            else
            {
                responce.message = "failed";
                responce.status = ResultCodeType.FAIL;
            }
            return Ok(responce);
        }

        [HttpPost]
        [Route("api/msit/createbuilding")]
        public IHttpActionResult CreateBuilding(Building building)
        {
            var identity = (ClaimsIdentity)User.Identity;
            var slist = model.createBuilding(building);
            ResponceData<dynamic> responce = new ResponceData<dynamic>();
            if (slist != false)
            {
                responce.Data = slist;
                responce.message = "success";
                responce.status = ResultCodeType.SUCCESS;
            }
            else
            {
                responce.message = "failed";
                responce.status = ResultCodeType.FAIL;
            }
            return Ok(responce);
        }


        [Route("api/msit/gettowndetailsdb")]
        public IHttpActionResult GetTownDetailsDB()
        {
            var identity = (ClaimsIdentity)User.Identity;
            var slist = model.GetTownListDB();
            ResponceData<dynamic> responce = new ResponceData<dynamic>();
            if (slist != null)
            {
                responce.Data = slist;
                responce.message = "success";
                responce.status = ResultCodeType.SUCCESS;
            }
            else
            {
                responce.message = "failed";
                responce.status = ResultCodeType.FAIL;
            }
            return Ok(responce);
        }


        [Route("api/msit/getfdrdetailsbytown")]
        public IHttpActionResult GetFdrDetailsbyTown(string town)
        {
            var identity = (ClaimsIdentity)User.Identity;
            var slist = model.GetFdrDetailsbyTown(town);
            ResponceData<dynamic> responce = new ResponceData<dynamic>();
            if (slist != null)
            {
                responce.Data = slist;
                responce.message = "success";
                responce.status = ResultCodeType.SUCCESS;
            }
            else
            {
                responce.message = "failed";
                responce.status = ResultCodeType.FAIL;
            }
            return Ok(responce);
        }


        [Route("api/msit/getdashboarddata")]
        public IHttpActionResult getdashboarddata(string townname = "NA")
        {
            var identity = (ClaimsIdentity)User.Identity;
            var slist = model.GetDashboardData(townname);
            ResponceData<dynamic> responce = new ResponceData<dynamic>();
            if (slist != null)
            {
                responce.Data = slist;
                responce.message = "success";
                responce.status = ResultCodeType.SUCCESS;
            }
            else
            {
                responce.message = "failed";
                responce.status = ResultCodeType.FAIL;
            }
            return Ok(responce);
        }


        [Route("api/msit/getextentbytown")]
        public IHttpActionResult GetExtentByTown(string townname = "NA", string fdrName = "NA")
        {
            var identity = (ClaimsIdentity)User.Identity;
            var slist = model.GetExtentByTown(townname, fdrName);
            ResponceData<dynamic> responce = new ResponceData<dynamic>();
            if (slist != null)
            {
                responce.Data = slist;
                responce.message = "success";
                responce.status = ResultCodeType.SUCCESS;
            }
            else
            {
                responce.message = "failed";
                responce.status = ResultCodeType.FAIL;
            }
            return Ok(responce);
        }



        [Route("api/msit/GetdtcDetailsbyTownandfdr")]
        public IHttpActionResult GetdtcDetailsbyTownandfdr(string town, string fdrname, string surtyp)
        {
            var identity = (ClaimsIdentity)User.Identity;
            var slist = model.GetdtcDetailsbyTownandfdr(town, fdrname, surtyp);
            ResponceData<dynamic> responce = new ResponceData<dynamic>();
            if (slist != null)
            {
                responce.Data = slist;
                responce.message = "success";
                responce.status = ResultCodeType.SUCCESS;
            }
            else
            {
                responce.message = "failed";
                responce.status = ResultCodeType.FAIL;
            }
            return Ok(responce);
        }


        [Route("api/msit/getCoordsOfDtr")]
        public IHttpActionResult getCoordsOfDtr(string ninId)
        {
            var identity = (ClaimsIdentity)User.Identity;
            var slist = model.getCoordsOfDtr(ninId);
            ResponceData<dynamic> responce = new ResponceData<dynamic>();
            if (slist != null)
            {
                responce.Data = slist;
                responce.message = "success";
                responce.status = ResultCodeType.SUCCESS;
            }
            else
            {
                responce.message = "failed";
                responce.status = ResultCodeType.FAIL;
            }
            return Ok(responce);
        }


        [Route("api/msit/getDtcGeom")]
        public IHttpActionResult getDtcGeom(string dtcname)
        {
            var identity = (ClaimsIdentity)User.Identity;
            var slist = model.getDtcGeom(dtcname);
            ResponceData<dynamic> responce = new ResponceData<dynamic>();
            if (slist != null)
            {
                responce.Data = slist;
                responce.message = "success";
                responce.status = ResultCodeType.SUCCESS;
            }
            else
            {
                responce.message = "failed";
                responce.status = ResultCodeType.FAIL;
            }
            return Ok(responce);
        }

        [Route("api/msit/createConsumerAndLtLine")]
        public IHttpActionResult createConsumerAndLtLine(consumer consumer)
        {
            var identity = (ClaimsIdentity)User.Identity;
            var slist = model.createConsumerAndLtLine(consumer);
            ResponceData<dynamic> responce = new ResponceData<dynamic>();
            if (slist != false)
            {
                responce.Data = slist;
                responce.message = "success";
                responce.status = ResultCodeType.SUCCESS;
            }
            else
            {
                responce.message = "failed";
                responce.status = ResultCodeType.FAIL;
            }
            return Ok(responce);
        }


        [Route("api/msit/GetBuildingIdByConsumer")]
        public IHttpActionResult GetBuildingIdByConsumer(string x, string y  )
        {
            var identity = (ClaimsIdentity)User.Identity;
            var slist = model.GetBuildingIdByConsumer(x,y);
            ResponceData<dynamic> responce = new ResponceData<dynamic>();
            if (slist != null)
            {
                responce.Data = slist;
                responce.message = "success";
                responce.status = ResultCodeType.SUCCESS;
            }
            else
            {
                responce.message = "failed";
                responce.status = ResultCodeType.FAIL;
            }
            return Ok(responce);
        }

        [Route("api/msit/AssignUserTask")]
        public IHttpActionResult AssignUserTask(usertask usertask)
        {
            var identity = (ClaimsIdentity)User.Identity;
            var slist = model.AssignUserTask(usertask);
            ResponceData<dynamic> responce = new ResponceData<dynamic>();
            if (slist != false)
            {
                responce.Data = slist;
                responce.message = "success";
                responce.status = ResultCodeType.SUCCESS;
            }
            else
            {
                responce.message = "failed";
                responce.status = ResultCodeType.FAIL;
            }
            return Ok(responce);
        }

        [Route("api/msit/GetSecNameList")]
        public IHttpActionResult GetSecNameList(string town)
        {
            var identity = (ClaimsIdentity)User.Identity;
            var slist = model.GetSecNameList(town);
            ResponceData<dynamic> responce = new ResponceData<dynamic>();
            if (slist != null)
            {
                responce.Data = slist;
                responce.message = "success";
                responce.status = ResultCodeType.SUCCESS;
            }
            else
            {
                responce.message = "failed";
                responce.status = ResultCodeType.FAIL;
            }
            return Ok(responce);
        }


        [Route("api/msit/GetDtrList")]
        public IHttpActionResult GetDtrList(string substionName, string feederName)
        {
            var identity = (ClaimsIdentity)User.Identity;
            var slist = model.GetDtrList(substionName, feederName);
            ResponceData<dynamic> responce = new ResponceData<dynamic>();
            if (slist != null)
            {
                responce.Data = slist;
                responce.message = "success";
                responce.status = ResultCodeType.SUCCESS;
            }
            else
            {
                responce.message = "failed";
                responce.status = ResultCodeType.FAIL;
            }
            return Ok(responce);
        }
    }
}
