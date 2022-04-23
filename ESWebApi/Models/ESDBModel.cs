using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ESWebApi.Models
{
    public class ESDBModel
    {
    }

    public class User
    {
        public int userid { get; set; }
        public string username { get; set; }
        public int usertypeid { get; set; }
        public string emailid { get; set; }
        public string mobileno { get; set; }
        public string adharcardno { get; set; }
        public bool isactive { get; set; }
        public DateTime create_date { get; set; }
    }


    public class CreateUser
    {
        public int userid { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public int usertypeid { get; set; }
        public string emailid { get; set; }
        public string mobileno { get; set; }
        public string adharcardno { get; set; }
        public bool isactive { get; set; }
        public DateTime create_date { get; set; }
    }


    class ResponceData<T>
    {
        public string message { get; set; }
        public ResultCodeType status { get; set; }
        public T Data { get; set; }
    }

    public enum ResultCodeType
    {
        SUCCESS = 200,
        FAIL = 400,
        LOADING = 2,
        WARNING = 3
    }


    public class Town
    {
        // public int id { get; set; }
        public string name { get; set; }
    }

    public class SurveyType
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class sectionName
    {

        public string name { get; set; }
        public string code { get; set; }
    }

    public class SubStation
    {
        // public int id { get; set; }
        public string name { get; set; }
    }

    public class sectionname
    {
        // public int id { get; set; }
        public string name { get; set; }
    }

    public class FeederName
    {
        public int id { get; set; }
        public string name { get; set; }
        public string code { get; set; }
        public string status { get; set; }
    }

    public class AssignTask
    {

        public int UserId { get; set; }
        public string Town { get; set; }
        public string SurveyType { get; set; }
        public string Substation_33 { get; set; }
        public string Substation_11 { get; set; }
        public string Feeder_33 { get; set; }
        public string Feeder_11 { get; set; }

        public string Feeder_code { get; set; }
        public int DTRID { get; set; }
        public int SurveyStatus { get; set; }

        public string feeder_code { get; set; }
        public string dtr_nin { get; set; }
        public string dt_name { get; set; }
    }

    public class pole
    {
        public string nin_id { get; set; }
        public string fdr_name { get; set; }
        public string sec_name { get; set; }
        public string pre_pol_no { get; set; }
        public string stud_pole { get; set; }
        public string earthing { get; set; }
        public string oth_cable { get; set; }
        public string status { get; set; }
        public string cut_point { get; set; }
        public string op_volt { get; set; }
        public string construct { get; set; }
        public string po_assem { get; set; }
        public string type { get; set; }
        public string net_arran { get; set; }
        public string rising { get; set; }
        public string sec_ins { get; set; }
        public string num_of_ins { get; set; }
        public string type_of_ins { get; set; }
        public string no_stay { get; set; }
        public string trans_ins { get; set; }
        public string crs_arm { get; set; }
        public string st_light { get; set; }
        public string sw_mt_ins { get; set; }
        public string tp_str_lig { get; set; }
        public string st_fit_wat { get; set; }
        public string st_fit_typ { get; set; }
        public string li_arr_ins { get; set; }
        public string geom { get; set; }
        public string sur_remark { get; set; }
        public string create_By { get; set; }
        public string create_Date { get; set; }
        public string substation_name { get; set; }
        public string survey_type { get; set; }
        public string con_to { get; set; }
        public string dtc_name { get; set; }
        public double x { get; set; }
        public double y { get; set; }
        public string town { get; set; }
        public string is_shared { get; set; }
        public string no_str_lig { get; set; }
        public string cro_arm_type { get; set; }

    }

    public class FeatureType
    {
        public string data { get; set; }
        public string featuretable { get; set; }
    }


    public class custom<T>
    {
        public T gendata { get; set; }
    }


    public class service_pillar
    {
        public string town { get; set; }
        public string nin_id { get; set; }
        public string fdr_name { get; set; }
        public string sec_name { get; set; }
        public string pre_pol_no { get; set; }
        public string earthing { get; set; }
        public string status { get; set; }
        public string op_volt { get; set; }
        public string construct { get; set; }
        public string type { get; set; }
        public string dtc_name { get; set; }
        public string no_of_w_fp { get; set; }
        public string sur_remark { get; set; }
        public string create_by { get; set; }
        public string create_date { get; set; }
        public string substation_name { get; set; }
        public string survey_type { get; set; }
        public string x { get; set; }
        public string y { get; set; }

    }

    public class feeder_pillar
    {
        public string town { get; set; }
        public string nin_id { get; set; }
        public string fdr_name { get; set; }
        public string dtc_name { get; set; }
        public string sec_name { get; set; }
        public string pre_pol_no { get; set; }
        public string x { get; set; }
        public string y { get; set; }
        public string earthing { get; set; }
        public string status { get; set; }
        public string op_volt { get; set; }
        public string construct { get; set; }
        public string type { get; set; }
        public string no_of_w_fp { get; set; }
        public string sur_remark { get; set; }
        public string create_by { get; set; }
        public string create_date { get; set; }
        public string substation_name { get; set; }
        public string survey_type { get; set; }
    }

    public class ht_line
    {
        public string town { get; set; }
        public string nin_id { get; set; }
        public string op_volt { get; set; }
        public string fdr_name { get; set; }
        public string pre_pol_no { get; set; }
        public string status { get; set; }
        public string no_of_wire { get; set; }
        public string phase { get; set; }
        public string ins_type { get; set; }
        public string cond_conf { get; set; }
        public string cond_type { get; set; }
        public string sur_remark { get; set; }
        public string create_by { get; set; }
        public string create_date { get; set; }
        public string substation_name { get; set; }
        public string survey_type { get; set; }
        public string sec_name { get; set; }
        public string from_x { get; set; }
        public string from_y { get; set; }
        public string to_x { get; set; }
        public string to_y { get; set; }

    }

    public class ht_cable
    {
        public string nin_id { get; set; }
        public string pre_pol_no { get; set; }
        public string op_volt { get; set; }
        public string fdr_name { get; set; }
        public string status { get; set; }
        public string phase { get; set; }
        public string ins_type { get; set; }
        public string cr_sec_ar { get; set; }
        public string no_core { get; set; }
        public string cab_mat { get; set; }
        public string con_pos { get; set; }
        public string met_no { get; set; }
        public string sec_name { get; set; }
        public string cab_typ { get; set; }
        public string sur_remark { get; set; }
        public string create_by { get; set; }
        public string create_date { get; set; }
        public string substation_name { get; set; }
        public string survey_type { get; set; }
        public string from_x { get; set; }
        public string from_y { get; set; }
        public string to_x { get; set; }
        public string to_y { get; set; }
        public string town { get; set; }


    }

    public class earth_switch
    {

        public string town { get; set; }
        public string nin_id { get; set; }
        public string conn { get; set; }
        public string status { get; set; }
        public string op_volt { get; set; }
        public string man_name { get; set; }
        public string man_s_no { get; set; }
        public string fdr_name { get; set; }
        public string man_date { get; set; }
        public string com_date { get; set; }
        public string phasing { get; set; }
        public string rat_curr { get; set; }
        public string nor_rat_cu { get; set; }
        public string ear_sw_typ { get; set; }
        public string sur_remark { get; set; }
        public string create_by { get; set; }
        public string create_date { get; set; }
        public string sec_name { get; set; }
        public string substation_name { get; set; }
        public string survey_type { get; set; }
        public string x { get; set; }
        public string y { get; set; }

    }

    public class dis_transformer
    {
        public string town { get; set; }
        public string nin_id { get; set; }
        public string con_to_pol { get; set; }
        public string earthing { get; set; }
        public string ear_type { get; set; }
        public string dt_name { get; set; }
        public string dtr_code { get; set; }
        public string sec_name { get; set; }
        public string st_name { get; set; }
        public string status { get; set; }
        public string man_name { get; set; }
        public string man_s_no { get; set; }
        public string man_date { get; set; }
        public string fencing { get; set; }
        public string type { get; set; }
        public string sub_type { get; set; }
        public string usage { get; set; }
        public string capacity { get; set; }
        public string hi_v_si_v { get; set; }
        public string lo_v_si_v { get; set; }
        public string cor_typ { get; set; }
        public string vec_grp { get; set; }
        public string per_imp { get; set; }
        public string lv_fdr_pr { get; set; }
        public string hv_pro { get; set; }
        public string phase { get; set; }
        public string lv_pro { get; set; }
        public string own_by { get; set; }
        public string no_of_lt_p { get; set; }
        public string no_oflt_fd { get; set; }
        public string met_no { get; set; }
        public string mod_num { get; set; }
        public string pre_of_tap { get; set; }
        public string cur_tap_po { get; set; }
        public string tap_ch_typ { get; set; }
        public string min_tap { get; set; }
        public string max_tap { get; set; }
        public string tp_ch_m_nm { get; set; }
        public string tp_ch_s_no { get; set; }
        public string tr_nm_phot { get; set; }
        public string sur_remark { get; set; }
        public string create_by { get; set; }
        public string create_date { get; set; }
        public string substation_name { get; set; }

        public string fdr_name { get; set; }
        public string survey_type { get; set; }
        public string x { get; set; }
        public string y { get; set; }

        public string image { get; set; }
        public string image_ext { get; set; }

    }

    public class rmu
    {
        public string town { get; set; }
        public string nin_id { get; set; }
        public string con_to_pol { get; set; }
        public string sec_name { get; set; }
        public string earthing { get; set; }
        public string st_name { get; set; }
        public string fdr_name { get; set; }
        public string status { get; set; }
        public string man_name { get; set; }
        public string man_s_no { get; set; }
        public string man_date { get; set; }
        public string rmu_type { get; set; }
        public string rmu_name { get; set; }
        public string op_volt { get; set; }
        public string con_pos { get; set; }
        public string sur_remark { get; set; }
        public string create_by { get; set; }
        public string tr_nm_phot { get; set; }
        public string create_date { get; set; }
        public string substation_name { get; set; }
        public string survey_type { get; set; }
        public string x { get; set; }
        public string y { get; set; }


    }

    public class fuse
    {
        public string town { get; set; }
        public string nin_id { get; set; }
        public string connected { get; set; }
        public string status { get; set; }
        public string op_volt { get; set; }
        public string type { get; set; }
        public string sec_name { get; set; }
        public string fdr_name { get; set; }
        public string phasing { get; set; }
        public string dtc_name { get; set; }
        public string rat_curr { get; set; }
        public string sur_remark { get; set; }
        public string create_by { get; set; }
        public string create_date { get; set; }
        public string substation_name { get; set; }
        public string survey_type { get; set; }
        public string x { get; set; }
        public string y { get; set; }

    }

    public class meter
    {
        public string town { get; set; }
        public string nin_id { get; set; }
        public string connected { get; set; }
        public string status { get; set; }
        public string op_volt { get; set; }
        public string sec_name { get; set; }
        public string fdr_name { get; set; }
        public string man_name { get; set; }
        public string man_s_no { get; set; }
        public string man_date { get; set; }
        public string com_date { get; set; }
        public string type { get; set; }
        public string ca_no { get; set; }
        public string mul_fac { get; set; }
        public string con_type { get; set; }
        public string met_no { get; set; }
        public string ct_rat { get; set; }
        public string pt_rat { get; set; }
        public string purpose { get; set; }
        public string mdi { get; set; }
        public string pow_fac { get; set; }
        public string sur_remark { get; set; }
        public string create_by { get; set; }
        public string create_date { get; set; }
        public string substation_name { get; set; }
        public string survey_type { get; set; }
        public string x { get; set; }
        public string y { get; set; }

    }

    public class lig_arrester
    {
        public string town { get; set; }
        public string nin_id { get; set; }
        public string connected { get; set; }
        public string status { get; set; }
        public string op_volt { get; set; }
        public string man_name { get; set; }
        public string fdr_name { get; set; }
        public string man_s_no { get; set; }
        public string man_date { get; set; }
        public string com_date { get; set; }
        public string type { get; set; }
        public string purpose { get; set; }
        public string dis_cur_cp { get; set; }
        public string phasing { get; set; }
        public string sur_remark { get; set; }
        public string create_by { get; set; }
        public string create_date { get; set; }
        public string substation_name { get; set; }
        public string sec_name { get; set; }
        public string survey_type { get; set; }
        public string x { get; set; }
        public string y { get; set; }

    }

    public class ct
    {
        public string town { get; set; }
        public string nin_id { get; set; }
        public string st_name { get; set; }
        public string fdr_name { get; set; }
        public string status { get; set; }
        public string op_volt { get; set; }
        public string sec_name { get; set; }
        public string phy_loc { get; set; }
        public string man_name { get; set; }
        public string man_s_no { get; set; }
        public string man_date { get; set; }
        public string com_date { get; set; }
        public string con_type { get; set; }
        public string ct_type { get; set; }
        public string no_of_core { get; set; }
        public string ratio { get; set; }
        public string sur_remark { get; set; }
        public string create_by { get; set; }
        public string create_date { get; set; }
        public string substation_name { get; set; }
        public string survey_type { get; set; }
        public string x { get; set; }
        public string y { get; set; }


    }

    public class pt
    {
        public string town { get; set; }
        public string nin_id { get; set; }
        public string sec_name { get; set; }
        public string st_name { get; set; }
        public string status { get; set; }
        public string fdr_name { get; set; }
        public string op_volt { get; set; }
        public string phy_loc { get; set; }
        public string man_name { get; set; }
        public string man_s_no { get; set; }
        public string man_date { get; set; }
        public string com_date { get; set; }
        public string con_type { get; set; }
        public string pt_type { get; set; }
        public string no_of_core { get; set; }
        public string ratio { get; set; }
        public string sur_remark { get; set; }
        public string create_by { get; set; }
        public string create_date { get; set; }
        public string substation_name { get; set; }
        public string survey_type { get; set; }
        public string x { get; set; }
        public string y { get; set; }

    }

    public class dis_box
    {
        public string town { get; set; }
        public string nin_id { get; set; }
        public string st_name { get; set; }
        public string status { get; set; }
        public string fdr_name { get; set; }
        public string man_name { get; set; }
        public string sec_name { get; set; }
        public string man_s_no { get; set; }
        public string man_date { get; set; }
        public string com_date { get; set; }
        public string op_volt { get; set; }
        public string rat_curr { get; set; }
        public string phase { get; set; }
        public string db_type { get; set; }
        public string sur_remark { get; set; }
        public string create_by { get; set; }
        public string create_date { get; set; }
        public string substation_name { get; set; }
        public string survey_type { get; set; }
        public string x { get; set; }
        public string y { get; set; }


    }
    public class tower_33
    {
        public string nin_id { get; set; }
        public string fdr_name { get; set; }
        public string sec_name { get; set; }
        public string earthing { get; set; }
        public string pre_pol_no { get; set; }
        public string status { get; set; }
        public string op_volt { get; set; }
        public string construct { get; set; }
        public string type { get; set; }
        public string sec_ins { get; set; }
        public string num_of_ins { get; set; }
        public string typ_insu { get; set; }
        public string no_circ { get; set; }
        public string he_of_tow { get; set; }
        public string sur_remark { get; set; }
        public string create_by { get; set; }
        public string create_date { get; set; }
        public string substation_name { get; set; }
        public string survey_type { get; set; }
        public string town { get; set; }
        public string x { get; set; }
        public string y { get; set; }
    }
    public class pole_info
    {
        public string nin_id { get; set; }
        public string x { get; set; }
        public string y { get; set; }
    }

    public class Building
    {
        public string nin_id { get; set; }
        public string substaion_name { get; set; }
        public string buildingGeomString { get; set; }
        public string bld_name { get; set; }
        public string address { get; set; }
        public string bld_type { get; set; }
        public string bld_num { get; set; }
        public string bld_r_num { get; set; }
        public string city { get; set; }
        public string con_status { get; set; }
        public string dist { get; set; }
        public string no_of_conn { get; set; }
        public string no_if_flat { get; set; }
        public string no_of_floor { get; set; }
        public string no_of_shop { get; set; }
        public string pincode { get; set; }
        public string soc_name { get; set; }
        public string sub_loc { get; set; }

        public string createdBy { get; set; }
        public string createdDate { get; set; }

    }

    public class consumer
    {
        public string nin_id { get; set; }
        public string bld_id { get; set; }

        public string dtr_name { get; set; }
        public string substation_name { get; set; }
        public string cus_no { get; set; }
        public string cus_name { get; set; }
        public string fat_name { get; set; }
        public string occ_name { get; set; }
        public string tower_no { get; set; }
        public string bld_name { get; set; }
        public string bloc_no { get; set; }
        public string sec_name { get; set; }
        public string loc_name { get; set; }
        public string rural { get; set; }
        public string city_name { get; set; }
        public string pincode { get; set; }
        public string san_load { get; set; }
        public string category { get; set; }
        public string sup_volt { get; set; }
        public string no_of_phase { get; set; }
        public string con_status { get; set; }
        public string met_no { get; set; }
        public string dtr_id { get; set; }
        public string fdr_name { get; set; }
        public string stat_name { get; set; }
        public string pole_id { get; set; }
        public string left_cus_no { get; set; }
        public string met_lo { get; set; }
        public string hig_mount { get; set; }
        public string view_glass { get; set; }
        public string met_type { get; set; }
        public string make_class { get; set; }
        public string seal_no { get; set; }
        public string seal { get; set; }
        public string pole_multi_fdr { get; set; }
        public string x { get; set; }
        public string y { get; set; }

        public string createdBy { get; set; }
        public string createdDate { get; set; }
    }

    public class app_version
    {
        public string app_type { get; set; }
        public string version { get; set; }
    }


    public class featuregeojson
    {
        public string layerName { get; set; }
        public string geoJson { get; set; }
        //string substaionname, string feedername, string surveytype
    }


    public class townDetails
    {
        public string townname { get; set; }
        public string feedercount_33 { get; set; }
        public string feedercount_11 { get; set; }
        public string dtc_count { get; set; }
        public string ht_length { get; set; }
        public string con_count { get; set; }
    }

    public class fdrDetails
    {
        public string feedername { get; set; }
        public string feedertype { get; set; }
        public string dtc_count { get; set; }
        public string ht_length { get; set; }
        public string con_count { get; set; }
        public string status { get; set; }
    }


    public class fdrtemp
    {
        public string feedername { get; set; }
        public string feedertype { get; set; }
        public string town { get; set; }
        public string status { get; set; }
    }

    public class dtcgeom
    {
        public string x { get; set; }
        public string y { get; set; }
        public string ninid { get; set; }
    }

    public class dtcDetails
    {
        public string dtcname { get; set; }
        public string dtcninid { get; set; }
        public string lt_len { get; set; }
        public string status { get; set; }
        public string con_count { get; set; }
    }

    public class lt_cable
    {

        public string nin_id { get; set; }
        public string pre_pol_no { get; set; }
        public string op_volt { get; set; }
        public string fdr_name { get; set; }
        public string status { get; set; }
        public string phase { get; set; }
        public string ins_type { get; set; }
        public string cr_sec_ar { get; set; }
        public string no_core { get; set; }
        public string cab_mat { get; set; }
        public string con_pos { get; set; }
        public string cab_typ { get; set; }
        public string met_no { get; set; }
        public string sur_remark { get; set; }
        public string dtr_name { get; set; }
        public string create_by { get; set; }
        public string create_date { get; set; }
        public string substation_name { get; set; }
        public string survey_type { get; set; }
        public string con_to { get; set; }

        public string dt_name { get; set; }
        public string curr_pole { get; set; }
        public string town { get; set; }
        public string from_x { get; set; }
        public string from_y { get; set; }
        public string to_x { get; set; }
        public string to_y { get; set; }
    }


    public class lt_line
    {
        public string nin_id { get; set; }
        public string op_volt { get; set; }
        public string fdr_name { get; set; }
        public string pre_pol_no { get; set; }
        public string status { get; set; }
        public string no_of_wire { get; set; }
        public string phase { get; set; }
        public string ins_type { get; set; }
        public string cond_conf { get; set; }
        public string c_c_type { get; set; }
        public string sur_remark { get; set; }
        public string dtr_name { get; set; }
        public string create_by { get; set; }
        public string create_date { get; set; }
        public string substation_name { get; set; }
        public string survey_type { get; set; }

        public string con_to { get; set; }

        public string dt_name { get; set; }
        public string curr_pole { get; set; }
        public string town { get; set; }
        
        public string from_x { get; set; }
        public string from_y { get; set; }
        public string to_x { get; set; }
        public string to_y { get; set; }

    }

    public class usertask {
        public string userid { get; set; }
        public string town { get; set; }
        public string substation_11 { get; set; }
        public string substation_33 { get; set; }
        public string surveytype { get; set; }
        public string feeder_33 { get; set; }
        public string feeder_11 { get; set; }
        public string section_name { get; set; }
        public string dtr_name { get; set; }
        public string dtr_nin { get; set; }
        public string surveystatus { get; set; }
        public string feeder_code { get; set; }
    }

    public class dtr_master {       
    public string dtc_name { get; set; }
        public string capacity { get; set; }

    }

    //public class usermaster { 
    
    //}
}