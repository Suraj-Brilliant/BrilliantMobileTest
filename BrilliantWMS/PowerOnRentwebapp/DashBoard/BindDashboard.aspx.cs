using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BrilliantWMS.PORDashboardService;
using System.Web.UI.DataVisualization.Charting;
using System.Data;
using BrilliantWMS.Login;

namespace BrilliantWMS.DashBoard
{
    public partial class BindDashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["ID"] != null)
            {
                BindDashboard1();
            }
        }

        protected void BindDashboard1()
        {
            iDashboardClient objService = new iDashboardClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                Chart1.Series.Clear();
                Chart1.ChartAreas.Clear();
                Chart1.Legends.Clear();
                Chart1.Titles.Clear();
                hdnDashboardID.Value = Request.QueryString["ID"].ToString();
                POR_Dashboard_UserWise objclsChartProperty = new POR_Dashboard_UserWise();
                objclsChartProperty = objService.GetDashboardsByDashboardID(Convert.ToInt64(Request.QueryString["ID"].ToString()), profile.DBConnection._constr);
                SetChartProperty(objclsChartProperty);
                try
                {
                    Chart1.DataBind();
                }
                catch (Exception ex)
                {
                    lblMsg.Text = "Error on Chart Binding-" + ex.Message;
                }

            }
            catch
            { }
            finally { objService.Close(); }
        }

        #region "Set Chart Property"
        private void SetChartProperty(POR_Dashboard_UserWise objclsChartProperty)
        {
            ChartArea chartArea = new ChartArea();
            try
            {
                //------------------------------------------------------------------------------------------------------
                //     Button1 = new ImageButton();
                //ImageButton1.Visible = objclsChartProperty.AllowPrint;

                Legend legend1 = new Legend();
                //// Set the Chart Properties

                Chart1.Width = Convert.ToInt32(objclsChartProperty.Width);
                Chart1.Height = Convert.ToInt32(objclsChartProperty.Height);


                Chart1.Palette = SetChartStyle(objclsChartProperty.ChartSyle);
                Chart1.BackColor = System.Drawing.Color.Transparent;
                Chart1.BorderSkin.SkinStyle = ChartBorderSkinStyle(objclsChartProperty);

                Chart1.BorderlineColor = System.Drawing.Color.AntiqueWhite;
                Chart1.BorderlineDashStyle = ChartDashStyle.Solid;
                Chart1.BorderlineWidth = 1;


                // Set the ChartArea properties
                //Chart1.Titles.Add(new Title(objclsChartProperty.ReportName, Docking.Top, new System.Drawing.Font("Verdana", 8f, FontStyle.Bold), System.Drawing.Color.Black));
                chartArea.BackColor = System.Drawing.Color.Transparent;
                chartArea.Area3DStyle.Enable3D = Convert.ToBoolean(objclsChartProperty.Is3D);
                if (Convert.ToBoolean(objclsChartProperty.Is3D) == true)
                {
                    chartArea.Area3DStyle.Rotation = Convert.ToInt32(objclsChartProperty.ChartArea3DRotation);
                    chartArea.Area3DStyle.IsClustered = true;

                }
                //// Add the ChartArea to the Chart

                Chart1.ChartAreas.Add(chartArea);
                Chart1.ChartAreas[0].AxisX.MajorGrid.Enabled = Convert.ToBoolean(objclsChartProperty.ShowGridLineX);
                Chart1.ChartAreas[0].AxisY.MajorGrid.Enabled = Convert.ToBoolean(objclsChartProperty.ShowGridLineY);
                if (Convert.ToBoolean(objclsChartProperty.IsLegend) == true)
                {
                    // Set the Legend properties

                    legend1.LegendItemOrder = LegendItemOrder.SameAsSeriesOrder;
                    legend1 = LegendPosition(objclsChartProperty.LegentPosition);
                    if (objclsChartProperty.LegentPosition.ToLower() == "left" || objclsChartProperty.LegentPosition.ToLower() == "right")
                    {
                        legend1.LegendStyle = LegendStyle.Column;
                    }
                    else
                    {

                        //IsDockedInsideChartArea="true" Name="Legend1" LegendStyle="Row" MaximumAutoSize="50" Alignment="Near" IsEquallySpacedItems="false" Font="Arial, 7.5pt, style=Bold" IsTextAutoFit="false"
                        legend1.LegendStyle = LegendStyle.Table;
                        legend1.Alignment = System.Drawing.StringAlignment.Near;
                        legend1.IsTextAutoFit = true;
                    }
                    legend1.Alignment = System.Drawing.StringAlignment.Center;
                    legend1.BackColor = System.Drawing.Color.Transparent;
                    legend1.BorderColor = System.Drawing.Color.Black;
                    legend1.BorderWidth = 1;
                    Chart1.Legends.Add(legend1);

                }

            }
            catch (Exception ex)
            {
                lblMsg.Text = "Error on Set Chart Property -" + ex.Message.ToString();
            }
            //----------------------------------------------------------------------------------------------------------------------
            SetChartType(objclsChartProperty, chartArea);

        }
        #endregion

        #region "Set Chart type"
        private void SetChartType(POR_Dashboard_UserWise objclsChartProperty, ChartArea chartArea)
        {
            switch (objclsChartProperty.Type.ToLower())
            {
                case "column":
                    BindColumnChart(objclsChartProperty, chartArea);
                    break;
                case "bar":
                    BindColumnChart(objclsChartProperty, chartArea);
                    break;
                case "pie":
                    BindPieChart(objclsChartProperty, chartArea);
                    break;
                case "funnel":
                    BindFunnelChart(objclsChartProperty, chartArea);
                    break;
                case "line":
                    BindLineChart(objclsChartProperty, chartArea);
                    break;
                case "stackedcolumn":
                    BindStackedColumn(objclsChartProperty, chartArea);
                    break;
                case "rangebar":
                    BindRangeBarChart(objclsChartProperty, chartArea);
                    break;
                //case "rangecolumn":
                //    BindRangeBarChart(objclsChartProperty, chartArea);
                //    break;
                default:
                    //BindCustomeChart(objclsChartProperty, chartArea);
                    break;
            }
        }

        #endregion

        #region "Set Skin Back Color Style"
        public BorderSkinStyle ChartBorderSkinStyle(POR_Dashboard_UserWise objclsChartProperty)
        {
            switch (objclsChartProperty.ChartBorderSkin.ToLower())
            {
                case "none":
                    return BorderSkinStyle.None;
                case "framethin1":
                    return BorderSkinStyle.FrameThin1;
                case "framethin2":
                    return BorderSkinStyle.FrameThin2;
                case "framethin3":
                    return BorderSkinStyle.FrameThin3;
                case "framethin4":
                    return BorderSkinStyle.FrameThin4;
                case "framethin5":
                    return BorderSkinStyle.FrameThin5;
                case "framethin6":
                    return BorderSkinStyle.FrameThin6;
                case "frametitle1":
                    return BorderSkinStyle.FrameTitle1;
                case "frametitle2":
                    return BorderSkinStyle.FrameTitle2;
                case "frametitle3":
                    return BorderSkinStyle.FrameTitle3;
                case "frametitle4":
                    return BorderSkinStyle.FrameTitle4;
                case "frametitle5":
                    return BorderSkinStyle.FrameTitle5;
                case "frametitle6":
                    return BorderSkinStyle.FrameTitle6;
                case "frametitle7":
                    return BorderSkinStyle.FrameTitle7;
                case "frametitle8":
                    return BorderSkinStyle.FrameTitle8;
                case "raised":
                    return BorderSkinStyle.Raised;
                case "sunken":
                    return BorderSkinStyle.Sunken;
                default:
                    return BorderSkinStyle.None;
            }
        }
        #endregion

        #region "Coloum Chart :Binding Method for Coloum Chart "

        private void BindColumnChart(POR_Dashboard_UserWise objclsChartProperty, ChartArea chartArea)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            iDashboardClient objService = new iDashboardClient();
            DataSet ds = new DataSet();
            ds = objService.GetDashboardDataByQuery(objclsChartProperty.DataQuery, profile.DBConnection._constr);
            double x = 0;
            try
            {
                for (int _intdsTcount = 0; _intdsTcount <= ds.Tables.Count - 1; _intdsTcount++)
                {

                    if (ds.Tables[_intdsTcount].Rows.Count > 0)
                    {
                        List<double> list = new List<double>();
                        String sx = "";
                        for (int i = 0; i <= ds.Tables[_intdsTcount].Rows.Count - 1; i++)
                        {
                            list.Add(Convert.ToDouble(ds.Tables[_intdsTcount].Rows[i][objclsChartProperty.Value].ToString()));
                            if (i == 0)
                            {
                                sx = sx + ds.Tables[_intdsTcount].Rows[i][objclsChartProperty.Text].ToString();
                            }
                            else
                            {
                                sx = sx + "," + ds.Tables[_intdsTcount].Rows[i][objclsChartProperty.Text].ToString();
                            }
                        }

                        double[] yValues = list.ToArray();
                        string[] xValues = sx.Split(',');

                        Series series = new Series(ds.Tables[_intdsTcount].Rows[0][objclsChartProperty.LegentValue].ToString());
                        series.ChartType = SetChartCustomeType(objclsChartProperty.Type);
                        series.IsValueShownAsLabel = true;
                        Chart1.BackGradientStyle = GradientStyle.TopBottom;
                        Chart1.BackColor = SetBGColor(objclsChartProperty.ChartBackgroundColor);
                        Chart1.ChartAreas[0].AxisX.Interval = 1;
                        Chart1.ChartAreas[0].AxisX.TextOrientation = TextOrientation.Auto;
                        Chart1.ChartAreas[0].AxisX.Title = objclsChartProperty.XLable;

                        Double temp = 0;
                        if (Convert.ToDouble(list.Max()) > Convert.ToDouble(objclsChartProperty.Height))
                        {
                            temp = Convert.ToDouble(list.Max()) / Convert.ToDouble(objclsChartProperty.Height);
                            temp = list.Max() / x;
                            if (x > temp || x == 1)
                            { x = temp; }
                        }
                        else
                        {

                            if (list.Max() != 1)
                            {
                                temp = Convert.ToDouble(objclsChartProperty.Height) / Convert.ToDouble(list.Max());
                                if (x > temp || x == 1)
                                {
                                    x = temp;
                                }
                                if (list.Max() == 0)
                                {
                                    x = 1;
                                }
                            }
                        }
                        Chart1.ChartAreas[0].AxisY.Interval = x;
                        Chart1.ChartAreas[0].AxisY.TextOrientation = TextOrientation.Auto;
                        Chart1.ChartAreas[0].AxisY.Title = objclsChartProperty.YLable;
                        Chart1.Series.Add(series);

                        Axis yAxis = new Axis(chartArea, AxisName.Y);
                        Axis xAxis = new Axis(chartArea, AxisName.X);

                        // Bind the data to the chart
                        Chart1.Series[ds.Tables[_intdsTcount].Rows[0][objclsChartProperty.LegentValue].ToString()].Points.DataBindXY(xValues, yValues);
                        Chart1.Series[ds.Tables[_intdsTcount].Rows[0][objclsChartProperty.LegentValue].ToString()].ToolTip = "" + objclsChartProperty.XLable + " \t= #VALX\n" + ds.Tables[_intdsTcount].Rows[0][objclsChartProperty.LegentValue].ToString() + "  " + objclsChartProperty.YLable + "\t= #VALY";
                    }
                }
                Chart1.DataBind();
            }
            catch (Exception ex)
            {
                lblMsg.Text = "Error on Coloum Chart Binding-" + ex.Message;
            }

            //BindGrid(objclsChartProperty);
        }

        #endregion

        #region "Pie Chart : Binding Method for pie chart"
        private void BindPieChart(POR_Dashboard_UserWise objclsChartProperty, ChartArea chartArea)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            iDashboardClient objService = new iDashboardClient();
            DataSet ds = new DataSet();
            ds = objService.GetDashboardDataByQuery(objclsChartProperty.DataQuery, profile.DBConnection._constr);
            try
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    List<double> list = new List<double>();
                    String sx = "";
                    for (int i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
                    {
                        list.Add(Convert.ToDouble(ds.Tables[0].Rows[i][objclsChartProperty.Value].ToString()));
                        if (i == 0)
                        {
                            sx = sx + ds.Tables[0].Rows[i][objclsChartProperty.Text].ToString();
                        }
                        else
                        {
                            sx = sx + "," + ds.Tables[0].Rows[i][objclsChartProperty.Text].ToString();
                        }
                    }

                    double[] yValues = list.ToArray();
                    string[] xValues = sx.Split(',');

                    // Build a pie series
                    Series series = new Series(ds.Tables[0].Rows[0][objclsChartProperty.LegentValue].ToString());
                    series.ChartType = SetChartCustomeType(objclsChartProperty.Type);
                    series.IsValueShownAsLabel = true;
                    Chart1.BackGradientStyle = GradientStyle.TopBottom;
                    Chart1.BackColor = SetBGColor(objclsChartProperty.ChartBackgroundColor);
                    Chart1.Series.Add(series);

                    Axis yAxis = new Axis(chartArea, AxisName.Y);
                    Axis xAxis = new Axis(chartArea, AxisName.X);

                    // Bind the data to the chart
                    Chart1.Series[ds.Tables[0].Rows[0][objclsChartProperty.LegentValue].ToString()].Points.DataBindXY(xValues, yValues);
                    Chart1.Series[ds.Tables[0].Rows[0][objclsChartProperty.LegentValue].ToString()].ToolTip = "" + objclsChartProperty.XLable + " \t= #VALX\n" + objclsChartProperty.YLable + "\t= #VALY";

                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = "Error on Pie Chart Binding-" + ex.Message;
            }

            //BindGrid(objclsChartProperty);
        }
        #endregion

        #region "Funnel Chart : Binding Method for funnel chart"
        private void BindFunnelChart(POR_Dashboard_UserWise objclsChartProperty, ChartArea chartArea)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            iDashboardClient objService = new iDashboardClient();
            DataSet ds = new DataSet();
            ds = objService.GetDashboardDataByQuery(objclsChartProperty.DataQuery, profile.DBConnection._constr);
            try
            {
                List<double> list = new List<double>();
                String sx = "";
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
                    {
                        list.Add(Convert.ToDouble(ds.Tables[0].Rows[i][objclsChartProperty.Value].ToString()));
                        if (i == 0)
                        {
                            sx = sx + ds.Tables[0].Rows[i][objclsChartProperty.Text].ToString();
                        }
                        else
                        {
                            sx = sx + "," + ds.Tables[0].Rows[i][objclsChartProperty.Text].ToString();
                        }
                    }

                    double[] yValues = list.ToArray();
                    string[] xValues = sx.Split(',');

                    // Build a pie series
                    Series series = new Series("Default");
                    series.ChartType = SeriesChartType.Funnel;

                    series.IsValueShownAsLabel = true;
                    Chart1.BackGradientStyle = GradientStyle.TopBottom;
                    Chart1.BackColor = SetBGColor(objclsChartProperty.ChartBackgroundColor);

                    Chart1.ChartAreas[0].AxisX.Interval = 1;
                    Chart1.ChartAreas[0].AxisX.TextOrientation = TextOrientation.Auto;
                    Chart1.ChartAreas[0].AxisX.Title = objclsChartProperty.Text;
                    Chart1.ChartAreas[0].AxisY.Interval = 1;
                    Chart1.ChartAreas[0].AxisY.TextOrientation = TextOrientation.Auto;
                    Chart1.ChartAreas[0].AxisY.Title = objclsChartProperty.Value;
                    Chart1.Series.Add(series);
                    Chart1.Series[0]["FunnelPointGap"] = "2";
                    Chart1.Series[0]["Funnel3DRotationAngle"] = objclsChartProperty.ChartArea3DRotation.ToString();
                    Axis yAxis = new Axis(chartArea, AxisName.Y);
                    Axis xAxis = new Axis(chartArea, AxisName.X);

                    // Bind the data to the chart
                    Chart1.Series["Default"].Points.DataBindXY(xValues, yValues);
                    Chart1.Series["Default"].ToolTip = "" + objclsChartProperty.XLable + " \t= #VALX\n" + objclsChartProperty.YLable + "\t= #VALY";
                    //BindGrid(objclsChartProperty);
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = "Error on Funnel Chart Binding-" + ex.Message;
            }

        }
        #endregion

        #region "Line Chart :Binding Method for Line Chart "
        private void BindLineChart(POR_Dashboard_UserWise objclsChartProperty, ChartArea chartArea)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            iDashboardClient objService = new iDashboardClient();
            DataSet ds = new DataSet();
            ds = objService.GetDashboardDataByQuery(objclsChartProperty.DataQuery, profile.DBConnection._constr);
            try
            {
                for (int _intdsTcount = 0; _intdsTcount <= ds.Tables.Count - 1; _intdsTcount++)
                {
                    if (ds.Tables[_intdsTcount].Rows.Count > 0)
                    {

                        List<double> list = new List<double>();
                        String sx = "";
                        for (int i = 0; i <= ds.Tables[_intdsTcount].Rows.Count - 1; i++)
                        {
                            list.Add(Convert.ToDouble(ds.Tables[_intdsTcount].Rows[i][objclsChartProperty.Value].ToString()));
                            if (i == 0)
                            {
                                sx = sx + ds.Tables[_intdsTcount].Rows[i][objclsChartProperty.Text].ToString();
                            }
                            else
                            {
                                sx = sx + "," + ds.Tables[_intdsTcount].Rows[i][objclsChartProperty.Text].ToString();
                            }
                        }

                        double[] yValues = list.ToArray();
                        string[] xValues = sx.Split(',');

                        Series series = new Series(ds.Tables[_intdsTcount].Rows[0][objclsChartProperty.LegentValue].ToString());
                        series.ChartType = SeriesChartType.Spline;
                        Chart1.BackGradientStyle = GradientStyle.TopBottom;
                        Chart1.BackColor = SetBGColor(objclsChartProperty.ChartBackgroundColor);
                        series.IsValueShownAsLabel = true;
                        Chart1.ChartAreas[0].AxisX.Interval = 1;
                        Chart1.ChartAreas[0].AxisX.TextOrientation = TextOrientation.Auto;
                        Chart1.ChartAreas[0].AxisX.Title = objclsChartProperty.XLable;
                        Chart1.ChartAreas[0].AxisY.Interval = 1;
                        Chart1.ChartAreas[0].AxisY.TextOrientation = TextOrientation.Auto;
                        Chart1.ChartAreas[0].AxisY.Title = objclsChartProperty.YLable;
                        series.BorderWidth = 5;
                        Chart1.Series.Add(series);

                        Axis yAxis = new Axis(chartArea, AxisName.Y);
                        Axis xAxis = new Axis(chartArea, AxisName.X);

                        // Bind the data to the chart
                        Chart1.Series[ds.Tables[_intdsTcount].Rows[0][objclsChartProperty.LegentValue].ToString()].Points.DataBindXY(xValues, yValues);
                        Chart1.Series[ds.Tables[_intdsTcount].Rows[0][objclsChartProperty.LegentValue].ToString()].ToolTip = "" + objclsChartProperty.XLable + " \t= #VALX\n" + ds.Tables[_intdsTcount].Rows[0][objclsChartProperty.LegentValue].ToString() + "  " + objclsChartProperty.YLable + "\t= #VALY";
                    }
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = "Error on Line Chart Binding-" + ex.Message;
            }

            //BindGrid(objclsChartProperty);

        }
        #endregion

        #region "Stock Column Chart :Bind Stock Column/Bar Chart"
        private void BindStackedColumn(POR_Dashboard_UserWise objclsChartProperty, ChartArea chartArea)
        {
            Double x = 1;
            CustomProfile profile = CustomProfile.GetProfile();
            iDashboardClient objService = new iDashboardClient();
            DataSet ds = new DataSet();
            ds = objService.GetDashboardDataByQuery(objclsChartProperty.DataQuery, profile.DBConnection._constr);
            try
            {
                for (int _intdsTcount = 0; _intdsTcount <= ds.Tables.Count - 1; _intdsTcount++)
                {
                    if (ds.Tables[_intdsTcount].Rows.Count > 0)
                    {
                        List<double> list = new List<double>();
                        String sx = "";
                        for (int i = 0; i <= ds.Tables[_intdsTcount].Rows.Count - 1; i++)
                        {
                            list.Add(Convert.ToDouble(ds.Tables[_intdsTcount].Rows[i][objclsChartProperty.Value].ToString()));
                            if (i == 0)
                            {
                                sx = sx + ds.Tables[_intdsTcount].Rows[i][objclsChartProperty.Text].ToString();
                            }
                            else
                            {
                                sx = sx + "," + ds.Tables[_intdsTcount].Rows[i][objclsChartProperty.Text].ToString();
                            }
                        }

                        double[] yValues = list.ToArray();
                        string[] xValues = sx.Split(',');

                        Series series = new Series(ds.Tables[_intdsTcount].Rows[0][objclsChartProperty.LegentValue].ToString());
                        series.ChartType = SetChartCustomeType(objclsChartProperty.Type);
                        series.IsValueShownAsLabel = true;
                        Chart1.BackGradientStyle = GradientStyle.TopBottom;
                        Chart1.BackColor = SetBGColor(objclsChartProperty.ChartBackgroundColor);
                        Chart1.ChartAreas[0].AxisX.Interval = 1;
                        Chart1.ChartAreas[0].AxisX.TextOrientation = TextOrientation.Auto;
                        Chart1.ChartAreas[0].AxisX.Title = objclsChartProperty.XLable;

                        Double temp = 0;
                        if (Convert.ToDouble(list.Max()) > Convert.ToDouble(objclsChartProperty.Height))
                        {
                            temp = Convert.ToDouble(list.Max()) / Convert.ToDouble(objclsChartProperty.Height);
                            temp = list.Max() / x;
                            if (x > temp || x == 1)
                            { x = temp; }
                        }
                        else
                        {

                            if (list.Max() != 1)
                            {
                                if (Convert.ToDouble(list.Max()) > Convert.ToDouble(objclsChartProperty.Height))
                                {
                                    temp = Convert.ToDouble(objclsChartProperty.Height) / Convert.ToDouble(list.Max());
                                    if (x > temp || x == 1)
                                    {
                                        x = temp;
                                    }
                                    if (list.Max() == 0)
                                    {
                                        x = 1;
                                    }
                                }
                                else
                                {
                                    x = 1;
                                }
                            }
                        }
                        Chart1.ChartAreas[0].AxisY.Interval = Math.Round(x);
                        Chart1.ChartAreas[0].AxisY.TextOrientation = TextOrientation.Auto;
                        Chart1.ChartAreas[0].AxisY.Title = objclsChartProperty.YLable;
                        Chart1.Series.Add(series);

                        Axis yAxis = new Axis(chartArea, AxisName.Y);
                        Axis xAxis = new Axis(chartArea, AxisName.X);

                        // Bind the data to the chart
                        Chart1.Series[ds.Tables[_intdsTcount].Rows[0][objclsChartProperty.LegentValue].ToString()].Points.DataBindXY(xValues, yValues);
                        Chart1.Series[ds.Tables[_intdsTcount].Rows[0][objclsChartProperty.LegentValue].ToString()].ToolTip = "" + objclsChartProperty.XLable + " \t= #VALX\n" + ds.Tables[_intdsTcount].Rows[0][objclsChartProperty.LegentValue].ToString() + "  " + objclsChartProperty.YLable + "\t= #VALY";
                    }

                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = "Error on Stock Chart Binding-" + ex.Message;
            }

            //BindGrid(objclsChartProperty);
        }
        #endregion

        #region "Range Bar Chart :Bind Range colume/Bar Chart"
        private void BindRangeBarChart(POR_Dashboard_UserWise objclsChartProperty, ChartArea chartArea)
        {
            double x = 0;
            CustomProfile profile = CustomProfile.GetProfile();
            iDashboardClient objService = new iDashboardClient();
            DataSet ds = new DataSet();
            ds = objService.GetDashboardDataByQuery(objclsChartProperty.DataQuery, profile.DBConnection._constr);
            try
            {
                for (int _intdsTcount = 0; _intdsTcount <= ds.Tables.Count - 1; _intdsTcount++)
                {
                    if (ds.Tables[_intdsTcount].Rows.Count > 0)
                    {
                        List<double> list = new List<double>();
                        String sx = "";
                        for (int i = 0; i <= ds.Tables[_intdsTcount].Rows.Count - 1; i++)
                        {
                            list.Add(Convert.ToDouble(ds.Tables[_intdsTcount].Rows[i][objclsChartProperty.Value].ToString()));
                            if (i == 0)
                            {
                                sx = sx + ds.Tables[_intdsTcount].Rows[i][objclsChartProperty.Text].ToString();
                            }
                            else
                            {
                                sx = sx + "," + ds.Tables[_intdsTcount].Rows[i][objclsChartProperty.Text].ToString();
                            }
                        }

                        double[] yValues = list.ToArray();
                        string[] xValues = sx.Split(',');

                        Series series = new Series(ds.Tables[_intdsTcount].Rows[0][objclsChartProperty.LegentValue].ToString());
                        series.ChartType = SetChartCustomeType(objclsChartProperty.Type);

                        series.IsXValueIndexed = true;


                        Chart1.BackGradientStyle = GradientStyle.TopBottom;
                        Chart1.BackColor = SetBGColor(objclsChartProperty.ChartBackgroundColor);
                        Chart1.ChartAreas[0].AxisX.Interval = 1;
                        Chart1.ChartAreas[0].AxisX.TextOrientation = TextOrientation.Auto;
                        Chart1.ChartAreas[0].AxisX.Title = objclsChartProperty.XLable;
                        Double temp = 0;
                        if (Convert.ToDouble(list.Max()) > Convert.ToDouble(objclsChartProperty.Height))
                        {
                            temp = Convert.ToDouble(list.Max()) / Convert.ToDouble(objclsChartProperty.Height);
                            temp = list.Max() / x;
                            if (x > temp || x == 1)
                            { x = temp; }
                        }
                        else
                        {

                            if (list.Max() != 1)
                            {
                                temp = Convert.ToDouble(objclsChartProperty.Height) / Convert.ToDouble(list.Max());
                                if (x > temp || x == 1)
                                {
                                    x = temp;
                                }
                                if (list.Max() == 0)
                                {
                                    x = 1;
                                }
                            }
                        }

                        Chart1.ChartAreas[0].AxisY.Interval = x;
                        Chart1.ChartAreas[0].AxisY.TextOrientation = TextOrientation.Auto;
                        Chart1.ChartAreas[0].AxisY.Title = objclsChartProperty.YLable;
                        Chart1.Series.Add(series);
                        if (_intdsTcount == 0) { Chart1.Series[0].CustomProperties = "PointWidth=0.7"; }
                        if (_intdsTcount >= 1) { Chart1.Series[_intdsTcount].CustomProperties = "PointWidth=0.4, DrawSideBySide=false"; }

                        Axis yAxis = new Axis(chartArea, AxisName.Y);
                        Axis xAxis = new Axis(chartArea, AxisName.X);

                        // Bind the data to the chart
                        Chart1.Series[ds.Tables[_intdsTcount].Rows[0][objclsChartProperty.LegentValue].ToString()].Points.DataBindXY(xValues, yValues);
                        Chart1.Series[ds.Tables[_intdsTcount].Rows[0][objclsChartProperty.LegentValue].ToString()].ToolTip = "" + objclsChartProperty.XLable + " \t= #VALX\n" + ds.Tables[_intdsTcount].Rows[0][objclsChartProperty.LegentValue].ToString() + "  " + objclsChartProperty.YLable + "\t= #VALY";
                    }
                }
                if (Convert.ToInt32(objclsChartProperty.Height) < ds.Tables[0].Rows.Count * 30) { Chart1.Height = ds.Tables[0].Rows.Count * 30; }
            }
            catch (Exception ex)
            {
                lblMsg.Text = "Error on Renage Chart Binding-" + ex.Message;
            }

            // BindGrid(objclsChartProperty);


        }
        #endregion

        #region "Set Back Color"
        public System.Drawing.Color SetBGColor(string ChartBackgroundColor)
        {
            switch (ChartBackgroundColor.ToLower())
            {

                case "aliceblue":
                    return System.Drawing.Color.AliceBlue;
                case "antiquewhite":
                    return System.Drawing.Color.AntiqueWhite;
                case "aqua":
                    return System.Drawing.Color.Aqua;
                case "aquamarine":
                    return System.Drawing.Color.Aquamarine;
                case "azure":
                    return System.Drawing.Color.Azure;
                case "beige":
                    return System.Drawing.Color.Beige;
                case "bisque":
                    return System.Drawing.Color.Bisque;
                case "black":
                    return System.Drawing.Color.Black;
                case "blanchedalmond":
                    return System.Drawing.Color.BlanchedAlmond;
                case "blue":
                    return System.Drawing.Color.Blue;
                case "blueviolet":
                    return System.Drawing.Color.BlueViolet;
                case "brown":
                    return System.Drawing.Color.Brown;
                case "burlywood":
                    return System.Drawing.Color.BurlyWood;
                case "cadetblue":
                    return System.Drawing.Color.CadetBlue;
                case "chartreuse":
                    return System.Drawing.Color.Chartreuse;
                case "chocolate":
                    return System.Drawing.Color.Chocolate;
                case "coral":
                    return System.Drawing.Color.Coral;
                case "cornflowerblue":
                    return System.Drawing.Color.CornflowerBlue;
                case "cornsilk":
                    return System.Drawing.Color.Cornsilk;
                case "crimson":
                    return System.Drawing.Color.Crimson;
                case "cyan":
                    return System.Drawing.Color.Cyan;
                case "darkblue":
                    return System.Drawing.Color.DarkBlue;
                case "darkcyan":
                    return System.Drawing.Color.DarkCyan;
                case "darkgoldenrod":
                    return System.Drawing.Color.DarkGoldenrod;
                case "darkgray":
                    return System.Drawing.Color.DarkGray;
                case "darkgreen":
                    return System.Drawing.Color.DarkGreen;
                case "darkkhaki":
                    return System.Drawing.Color.DarkKhaki;
                case "darkmagenta":
                    return System.Drawing.Color.DarkMagenta;
                case "darkolivegreen":
                    return System.Drawing.Color.DarkOliveGreen;
                case "darkorange":
                    return System.Drawing.Color.DarkOrange;
                case "darkorchid":
                    return System.Drawing.Color.DarkOrchid;
                case "darkred":
                    return System.Drawing.Color.DarkRed;
                case "darksalmon":
                    return System.Drawing.Color.DarkSalmon;
                case "darkseagreen":
                    return System.Drawing.Color.DarkSeaGreen;
                case "darkslateblue":
                    return System.Drawing.Color.DarkSlateBlue;
                case "darkslategray":
                    return System.Drawing.Color.DarkSlateGray;
                case "darkturquoise":
                    return System.Drawing.Color.DarkTurquoise;
                case "darkviolet":
                    return System.Drawing.Color.DarkViolet;
                case "deeppink":
                    return System.Drawing.Color.DeepPink;
                case "deepskyblue":
                    return System.Drawing.Color.DeepSkyBlue;
                case "dimgray":
                    return System.Drawing.Color.DimGray;
                case "dodgerblue":
                    return System.Drawing.Color.DodgerBlue;
                case "firebrick":
                    return System.Drawing.Color.Firebrick;
                case "floralwhite":
                    return System.Drawing.Color.FloralWhite;
                case "forestgreen":
                    return System.Drawing.Color.ForestGreen;
                case "fuchsia":
                    return System.Drawing.Color.Fuchsia;
                case "gainsboro":
                    return System.Drawing.Color.Gainsboro;
                case "ghostwhite":
                    return System.Drawing.Color.GhostWhite;
                case "gold":
                    return System.Drawing.Color.Gold;
                case "goldenrod":
                    return System.Drawing.Color.Goldenrod;
                case "gray":
                    return System.Drawing.Color.Gray;
                case "green":
                    return System.Drawing.Color.Green;
                case "greenyellow":
                    return System.Drawing.Color.GreenYellow;
                case "honeydew":
                    return System.Drawing.Color.Honeydew;
                case "hotpink":
                    return System.Drawing.Color.HotPink;
                case "indianred":
                    return System.Drawing.Color.IndianRed;
                case "indigo":
                    return System.Drawing.Color.Indigo;
                case "ivory":
                    return System.Drawing.Color.Ivory;
                case "khaki":
                    return System.Drawing.Color.Khaki;
                case "lavender":
                    return System.Drawing.Color.Lavender;
                case "lavenderblush":
                    return System.Drawing.Color.LavenderBlush;
                case "lawngreen":
                    return System.Drawing.Color.LawnGreen;
                case "lemonchiffon":
                    return System.Drawing.Color.LemonChiffon;
                case "lightblue":
                    return System.Drawing.Color.LightBlue;
                case "lightcoral":
                    return System.Drawing.Color.LightCoral;
                case "lightcyan":
                    return System.Drawing.Color.LightCyan;
                case "lightgoldenrodyellow":
                    return System.Drawing.Color.LightGoldenrodYellow;
                case "lightgray":
                    return System.Drawing.Color.LightGray;
                case "lightgreen":
                    return System.Drawing.Color.LightGreen;
                case "lightpink":
                    return System.Drawing.Color.LightPink;
                case "lightsalmon":
                    return System.Drawing.Color.LightSalmon;
                case "lightseagreen":
                    return System.Drawing.Color.LightSeaGreen;
                case "lightskyblue":
                    return System.Drawing.Color.LightSkyBlue;
                case "lightslategray":
                    return System.Drawing.Color.LightSlateGray;
                case "lightsteelblue":
                    return System.Drawing.Color.LightSteelBlue;
                case "lightyellow":
                    return System.Drawing.Color.LightYellow;
                case "lime":
                    return System.Drawing.Color.Lime;
                case "limegreen":
                    return System.Drawing.Color.LimeGreen;
                case "linen":
                    return System.Drawing.Color.Linen;
                case "magenta":
                    return System.Drawing.Color.Magenta;
                case "maroon":
                    return System.Drawing.Color.Maroon;
                case "mediumaquamarine":
                    return System.Drawing.Color.MediumAquamarine;
                case "mediumblue":
                    return System.Drawing.Color.MediumBlue;
                case "mediumorchid":
                    return System.Drawing.Color.MediumOrchid;
                case "mediumpurple":
                    return System.Drawing.Color.MediumPurple;
                case "mediumseagreen":
                    return System.Drawing.Color.MediumSeaGreen;
                case "mediumslateblue":
                    return System.Drawing.Color.MediumSlateBlue;
                case "mediumspringgreen":
                    return System.Drawing.Color.MediumSpringGreen;
                case "mediumturquoise":
                    return System.Drawing.Color.MediumTurquoise;
                case "mediumvioletred":
                    return System.Drawing.Color.MediumVioletRed;
                case "midnightblue":
                    return System.Drawing.Color.MidnightBlue;
                case "mintcream":
                    return System.Drawing.Color.MintCream;
                case "mistyrose":
                    return System.Drawing.Color.MistyRose;
                case "moccasin":
                    return System.Drawing.Color.Moccasin;
                case "navajowhite":
                    return System.Drawing.Color.NavajoWhite;
                case "navy":
                    return System.Drawing.Color.Navy;
                case "oldlace":
                    return System.Drawing.Color.OldLace;
                case "olive":
                    return System.Drawing.Color.Olive;
                case "olivedrab":
                    return System.Drawing.Color.OliveDrab;
                case "orange":
                    return System.Drawing.Color.Orange;
                case "orangered":
                    return System.Drawing.Color.OrangeRed;
                case "orchid":
                    return System.Drawing.Color.Orchid;
                case "palegoldenrod":
                    return System.Drawing.Color.PaleGoldenrod;
                case "palegreen":
                    return System.Drawing.Color.PaleGreen;
                case "paleturquoise":
                    return System.Drawing.Color.PaleTurquoise;
                case "palevioletred":
                    return System.Drawing.Color.PaleVioletRed;
                case "papayawhip":
                    return System.Drawing.Color.PapayaWhip;
                case "peachpuff":
                    return System.Drawing.Color.PeachPuff;
                case "peru":
                    return System.Drawing.Color.Peru;
                case "pink":
                    return System.Drawing.Color.Pink;
                case "plum":
                    return System.Drawing.Color.Plum;
                case "powderblue":
                    return System.Drawing.Color.PowderBlue;
                case "purple":
                    return System.Drawing.Color.Purple;
                case "red":
                    return System.Drawing.Color.Red;
                case "rosybrown":
                    return System.Drawing.Color.RosyBrown;
                case "royalblue":
                    return System.Drawing.Color.RoyalBlue;
                case "saddlebrown":
                    return System.Drawing.Color.SaddleBrown;
                case "salmon":
                    return System.Drawing.Color.Salmon;
                case "sandybrown":
                    return System.Drawing.Color.SandyBrown;
                case "seagreen":
                    return System.Drawing.Color.SeaGreen;
                case "seashell":
                    return System.Drawing.Color.SeaShell;
                case "sienna":
                    return System.Drawing.Color.Sienna;
                case "silver":
                    return System.Drawing.Color.Silver;
                case "skyblue":
                    return System.Drawing.Color.SkyBlue;
                case "slateblue":
                    return System.Drawing.Color.SlateBlue;
                case "slategray":
                    return System.Drawing.Color.SlateGray;
                case "snow":
                    return System.Drawing.Color.Snow;
                case "springgreen":
                    return System.Drawing.Color.SpringGreen;
                case "steelblue":
                    return System.Drawing.Color.SteelBlue;
                case "tan":
                    return System.Drawing.Color.Tan;
                case "teal":
                    return System.Drawing.Color.Teal;
                case "thistle":
                    return System.Drawing.Color.Thistle;
                case "tomato":
                    return System.Drawing.Color.Tomato;
                case "transparent":
                    return System.Drawing.Color.Transparent;
                case "turquoise":
                    return System.Drawing.Color.Turquoise;
                case "violet":
                    return System.Drawing.Color.Violet;
                case "wheat":
                    return System.Drawing.Color.Wheat;
                case "white":
                    return System.Drawing.Color.White;
                case "whitesmoke":
                    return System.Drawing.Color.WhiteSmoke;
                case "yellow":
                    return System.Drawing.Color.Yellow;
                case "yellowgreen":
                    return System.Drawing.Color.YellowGreen;
                default:
                    return System.Drawing.Color.YellowGreen;

            }
        }
        #endregion

        #region "Return Custome chart type"
        public SeriesChartType SetChartCustomeType(String _strType)
        {
            SeriesChartType x = new SeriesChartType();

            switch (_strType.ToLower())
            {
                case "area":
                    x = SeriesChartType.Area;
                    break;
                case "bar":
                    x = SeriesChartType.Bar;
                    break;
                case "boxplot":
                    x = SeriesChartType.BoxPlot;
                    break;
                case "stepline":
                    x = SeriesChartType.StepLine;
                    break;
                case "bubble":
                    x = SeriesChartType.Bubble;
                    break;
                case "candlestick":
                    x = SeriesChartType.Candlestick;
                    break;
                case "column":
                    x = SeriesChartType.Column;
                    break;
                case "doughnut":
                    x = SeriesChartType.Doughnut;
                    break;
                case "errorbar":
                    x = SeriesChartType.ErrorBar;
                    break;
                case "fastline":
                    x = SeriesChartType.FastLine;
                    break;
                case "fastpoint":
                    x = SeriesChartType.FastPoint;
                    break;
                case "funnel":
                    x = SeriesChartType.Funnel;
                    break;
                case "kagi":
                    x = SeriesChartType.Kagi;
                    break;
                case "line":
                    x = SeriesChartType.Line;
                    break;
                case "pie":
                    x = SeriesChartType.Pie;
                    break;
                case "point":
                    x = SeriesChartType.Point;
                    break;
                case "pointandfigure":
                    x = SeriesChartType.PointAndFigure;
                    break;
                case "polar":
                    x = SeriesChartType.Polar;
                    break;
                case "pyramid":
                    x = SeriesChartType.Pyramid;
                    break;
                case "radar":
                    x = SeriesChartType.Radar;
                    break;
                case "range":
                    x = SeriesChartType.Range;
                    break;
                case "rangebar":
                    x = SeriesChartType.RangeBar;
                    break;
                case "rangecolumn":
                    x = SeriesChartType.RangeColumn;
                    break;
                case "renko":
                    x = SeriesChartType.Renko;
                    break;
                case "spline":
                    x = SeriesChartType.Spline;
                    break;
                case "splinearea":
                    x = SeriesChartType.SplineArea;
                    break;
                case "splinerange":
                    x = SeriesChartType.SplineRange;
                    break;
                case "stackedarea":
                    x = SeriesChartType.StackedArea;
                    break;
                case "stackedarea100":
                    x = SeriesChartType.StackedArea100;
                    break;
                case "stackedbar":
                    x = SeriesChartType.StackedBar;
                    break;
                case "stackedbar100":
                    x = SeriesChartType.StackedBar100;
                    break;
                case "stackedcolumn":
                    x = SeriesChartType.StackedColumn;
                    break;
                case "stackedcolumn100":
                    x = SeriesChartType.StackedColumn100;
                    break;
                case "stock":
                    x = SeriesChartType.Stock;
                    break;
                case "threelinebreak":
                    x = SeriesChartType.ThreeLineBreak;
                    break;

            }
            return x;
        }
        #endregion

        #region "Return chart style"
        public ChartColorPalette SetChartStyle(String _strStyle)
        {
            ChartColorPalette objPattern = new ChartColorPalette();

            switch (_strStyle.ToLower())
            {
                case "berry":
                    objPattern = ChartColorPalette.Berry;
                    break;
                case "bright":
                    objPattern = ChartColorPalette.Bright;
                    break;
                case "brightpastel":
                    objPattern = ChartColorPalette.BrightPastel;
                    break;
                case "chocolate":
                    objPattern = ChartColorPalette.Chocolate;
                    break;
                case "earthtones":
                    objPattern = ChartColorPalette.EarthTones;
                    break;
                case "excel":
                    objPattern = ChartColorPalette.Excel;
                    break;
                case "fire":
                    objPattern = ChartColorPalette.Fire;
                    break;
                case "grayscale":
                    objPattern = ChartColorPalette.Grayscale;
                    break;
                case "light":
                    objPattern = ChartColorPalette.Light;
                    break;
                case "none":
                    objPattern = ChartColorPalette.None;
                    break;
                case "pastel":
                    objPattern = ChartColorPalette.Pastel;
                    break;
                case "seagreen":
                    objPattern = ChartColorPalette.SeaGreen;
                    break;
                case "semitransparent":
                    objPattern = ChartColorPalette.SemiTransparent;
                    break;
                default:
                    objPattern = ChartColorPalette.BrightPastel;
                    break;
            }


            return objPattern;
        }

        #endregion

        #region "Return chart Legend Position"
        public Legend LegendPosition(String _strPosition)
        {
            Legend Legend = new Legend();
            switch (_strPosition.ToLower())
            {
                case "bottom":
                    Legend.Docking = Docking.Bottom;
                    break;
                case "left":
                    Legend.Docking = Docking.Left;
                    break;
                case "right":
                    Legend.Docking = Docking.Right;
                    break;
                case "top":
                    Legend.Docking = Docking.Top;
                    break;
                default:
                    Legend.Docking = Docking.Bottom;
                    break;

            }
            return Legend;
        }

        #endregion

    }
}