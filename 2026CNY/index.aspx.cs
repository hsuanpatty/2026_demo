using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _2025_events_CNY_index : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack == false)
        {
            fn_Show_Data();
        }
    }
    protected void fn_Show_Data()
    {
        string strSql = "";
        strSql = " select Grop_Numb,Grop_Name,Grop_Depa,Grop_Day,Grop_Liner";
        strSql += " ,isnull(Grop_JoinTour,0)+isnull(Grop.Reg_INF,0)+isnull(Grop.Reg_FIT,0)+isnull(Grop_Expect,0) as [Grop_Expect],Grop_Visa,Grop_Tax,Grop_Tour,Grop_Intro";
        strSql += " ,area.Area_Name,grop.Trip_No,Grop_Number,reg_ok,reg_standby";
        strSql += " ,reg_checkok,Area.area_no,grop_pdf,grop_close,grop_ok";
        strSql += " ,IsNull(pak,0) as pak,Group_Name,Grop.Grop_JoinTour,reg_reserve,grop.Group_Category_No";
        strSql += " ,Van_Number,Grop.Tour_Kind,Grop.Group_Name_No,Grop.TourType,Grop.Reg_INF,Grop.Reg_FIT,trip.trip_early_bird_url";
        strSql += " ,Grop.group_standby,grop.UnityGroup";
        strSql += " ,Pak_ArCheck_Sync,Pak_SignUp_Sync,Grop.Source_Agent_No,Right(datename(weekday,Grop_Depa),1) as wd,area.Area_ID";
        strSql += " ,ROW_NUMBER() OVER(PARTITION BY Grop_Depa ORDER BY Grop_Depa) as order_row_id"; //'訂算相同訂單的順序
        strSql += " ,COUNT(1) over(PARTITION BY Grop_Depa) order_row_cnt"; // '計算相同訂單的總筆數
        strSql += " From Grop";
        strSql += " LEFT JOIN trip on trip.trip_no = grop.trip_no";
        strSql += " LEFT JOIN Area ON Area.Area_ID = GROP.Area_Code";
        strSql += " LEFT JOIN Group_Name ON Group_Name.Group_Name_No = Grop.Group_Name_No";
        strSql += " LEFT JOIN Tour_Price ON Tour_Price.Number = Grop.Van_Number and Tour_Price.Tick_Type = 'Cruises' and Tour_Price.adult_agent <> 0";
        strSql += " where 1=1";
        strSql += " and Area.Area_ID <> 'Area18'";
        strSql += " and isnull(hidden,'') <> 'y'";
        strSql += " and Trip.Trip_Hide=0";
        strSql += " and Grop.CANC_PEOL = ''";
        strSql += " and (Grop.ShowWeb = 0 OR Grop.ShowWeb = 1)";
        strSql += " and Grop_Depa >= @Grop_Depa_1";
        strSql += " and Grop_Depa <= @Grop_Depa_2";
        //strSql += " and Grop.TourType2 IN (N',過年,', N',極光,')";
        // 20240819 roger begin
        //strSql += " and (CHARINDEX(',過年,', Grop.TourType2) > 0 or CHARINDEX(',極光,', Grop.TourType2) > 0)";
        // 20240819 roger end
        //strSql += " and Trip.Area_No = @Area_No";

        // GROUP BY
        strSql += " group by Grop_Numb,Van_Number,Grop_Name,Grop_Depa,Grop_Day";
        strSql += " ,Grop_Liner,Grop_Expect,Grop_Visa,Grop_Tax,Grop_Tour";
        strSql += " ,Grop_Intro,area.Area_Name,grop.Trip_No,Grop_Number,reg_ok";
        strSql += " ,reg_standby,reg_checkok,Area.area_no,grop_pdf,grop_close";
        strSql += " ,grop_ok,pak,Group_Name,Agent_tour,reg_reserve";
        strSql += " ,grop.Group_Category_No,Grop.Tour_Kind,grop.Grop_JoinTour,Grop.Group_Name_No,Grop.TourType,Grop.Reg_INF,Grop.Reg_FIT,trip.trip_early_bird_url";
        strSql += " ,Grop.group_standby,grop.UnityGroup";
        strSql += " ,Pak_ArCheck_Sync,Pak_SignUp_Sync,Grop.Source_Agent_No,area.Area_ID,area.Array";
        strSql += " ORDER BY Grop_Depa,area.Array,Trip_No,Grop_Numb";
        string strConnString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["TRIPConnectionString"].ToString();
        SqlConnection connect = new SqlConnection(strConnString);
        connect.Open();
        SqlDataAdapter da = new SqlDataAdapter(strSql, strConnString);
        da.SelectCommand.Parameters.Add(new SqlParameter("@Grop_Depa_1", "2026-02-01"));
        da.SelectCommand.Parameters.Add(new SqlParameter("@Grop_Depa_2", "2026-02-28"));

        DataTable dt = new DataTable();
        da.Fill(dt);
        connect.Close();

        // ******************************************************************
        // 自定義分類


        // ******************************************************************
        // 顯示大地區
        var query = from row in dt.AsEnumerable()
                        //.GroupBy(r => new { Col1 = r["Col1"], Col2 = r["Col2"] })
                        //group row by row.Field<string>("name") into grp
                    group row by new { area_id = row.Field<string>("Area_ID"), area_name = row.Field<string>("Area_Name") } into grp
                    // 按照区域名进行排序
                    select new
                    {
                        area_id = grp.Key.area_id,
                        areaname = grp.Key.area_name
                    };

        string[] OrderAreaID = new[] { "Area1", "Area2", "Area3", "Area4", "Area5", "Area6", "Area10"/*美洲*/, "Area7", "Area8", "Area9", "Area11", "Area12"
                                    ,  "Area16"/*日本*/, "Area13","Area14","Area15","Area17", "Area18", "Area19", "Area20", "Area21" };
        var sortedQuery = query.OrderBy(item => Array.IndexOf(OrderAreaID, item.area_id))
                               .ThenBy(item => item.area_id)
                               .ThenBy(item => item.areaname);


        foreach (var grp in sortedQuery)
        {
            //Response.Write(String.Format("The Sum of '{0}' is {1}", grp.Id, grp.sum));
            litAreaList.Text += "<li class='buttons'>";
            litAreaList.Text += "<button class='btn' onclick=\"filterSelection('" + grp.area_id + "'); updateLastVisibleLink();\">";
            litAreaList.Text += grp.areaname;
            litAreaList.Text += "</button>";
            litAreaList.Text += "</li>";
        }

        // ******************************************************************
        // 顯示行程
        string AreaNo;
        string lblcolor;
        litDayList.Text = "";

        DateTime newyear = new DateTime(2025, 1, 2);
        DateTime endnewyear = new DateTime(2025, 2, 21);
        DateTime dtThisNow = Convert.ToDateTime("2026-02-01");

        while (dtThisNow <= Convert.ToDateTime("2026-02-28"))
        {
            DataRow[] selectedRows = dt.Select("Grop_Depa = '" + dtThisNow.ToString("yyyy/MM/dd") + "'");
            string dayAbbreviation = dtThisNow.ToString("ddd", new System.Globalization.CultureInfo("zh-tw")).Replace("週", "");
            if (dtThisNow >= newyear && dtThisNow <= endnewyear)
            {
                lblcolor = "red";
            }
            else if (dtThisNow.DayOfWeek >= DayOfWeek.Monday && dtThisNow.DayOfWeek <= DayOfWeek.Friday)
            {
                lblcolor = "yellow";
            }
            else
            {
                lblcolor = "blue";
            }

            if (selectedRows.Length == 0)
            {

                // 列出空白的 div
                litDayList.Text += "<div class='day noshow'>";
                litDayList.Text += "   <div class='date " + lblcolor + "'>" + dtThisNow.ToString("MM /dd") + "(" + dayAbbreviation + ")</div>";
                litDayList.Text += "   <div class='list'>";
                litDayList.Text += "      </div>";
                litDayList.Text += "</div>";
            }
            else
            {
                for (int ii = 0; ii < selectedRows.Length; ii++)
                {
                    DateTime dtGrop_Depa = Convert.ToDateTime(selectedRows[ii]["Grop_Depa"].ToString());
                    AreaNo = selectedRows[ii]["Area_No"].ToString();
                    if (selectedRows[ii]["Grop_Liner"].ToString() == "")
                    {
                        selectedRows[ii]["Grop_Liner"] = "none";
                    }


                    // 執行其他操作
                    if (selectedRows[ii]["order_row_id"].ToString() == "1")
                    {
                        litDayList.Text += "<div class='day " + "" + "'>";
                        litDayList.Text += "   <div class='date " + lblcolor + "'>" + dtThisNow.ToString("MM/dd") + "(" + dayAbbreviation + ")</div>";
                        litDayList.Text += "   <div class='list'>";
                    }
                    litDayList.Text += "      <div class='list-title filterDiv " + selectedRows[ii]["Area_ID"].ToString() + "'>";
                    if (selectedRows[ii]["trip_early_bird_url"].ToString() != "")
                    {
                        litDayList.Text += "      <a href = '" + selectedRows[ii]["trip_early_bird_url"].ToString() + "</a>";
                    }
                    else if (string.IsNullOrEmpty(selectedRows[ii]["Grop_Pdf"].ToString()))
                    {
                        if (selectedRows[ii]["Grop_Liner"].ToString() == "") { selectedRows[ii]["Grop_Liner"] = "none"; }
                        litDayList.Text += "      <a href = '/TripIntroduction.aspx?TripNo=" + selectedRows[ii]["Trip_No"].ToString() + "&Date=" + dtGrop_Depa.ToString("yyyy/MM/dd") + "&type=" + selectedRows[ii]["Grop_Liner"].ToString() + "'>" + selectedRows[ii]["Group_Name"].ToString() + "</a>";
                    }
                    else
                    {
                        litDayList.Text += "      <a href = '/GropPDF/" + selectedRows[ii]["Grop_Pdf"].ToString() + "'>" + selectedRows[ii]["Group_Name"].ToString() + "</a>";
                    }
                    litDayList.Text += "      </div>";

                    if (selectedRows[ii]["order_row_id"].ToString() == selectedRows[ii]["order_row_cnt"].ToString())
                    {
                        litDayList.Text += "   </div>";
                        litDayList.Text += "</div>";
                    }
                }
            }
            dtThisNow = dtThisNow.AddDays(1);
        }

    }
}