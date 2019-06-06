using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.DataVisualization.Charting;
using System.Data;
using System.Data.SqlClient;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Drawing;


namespace BrilliantWMS.DashBoard
{
    public partial class DashBoard : System.Web.UI.UserControl
    {

        protected void Page_Load(object sender, EventArgs e)
        {
          
        }

        public String _strConn;
        ChartProperty objclsChartPropertyOnPage = new ChartProperty();
        DataSet ds = new DataSet();

        #region "For Manual Chart Binding"
        private void BindChartManually(ChartProperty objclsChartProperty)
        {
            Chart chart;
            ChartArea chartArea;
            //SetChartProperty(objclsChartProperty, out chart, out chartArea);
            //SetChartType(objclsChartProperty, chart, chartArea);
        }
        #endregion

        #region "Bind chart from database"
        public void BindChart(ChartProperty objclsChartProperty)
        {
            Chart1.Series.Clear();
            Chart1.ChartAreas.Clear();
            Chart1.Legends.Clear();
            Chart1.Titles.Clear();
            hfRepeortid.Value = objclsChartProperty.ReportID.ToString();
            hfQueryParameter.Value = objclsChartProperty.QueryParameter;
            hfConnectionString.Value = objclsChartProperty.ConnectionString;
            hfQueryFlag.Value = objclsChartProperty.IsQueryManualy.ToString();
            if (objclsChartProperty.IsQueryManualy == true && objclsChartProperty.ExportChart == false)
            {
                hfQuery.Value = objclsChartProperty.Query.ToString();
            }
            //check report data
            if (objclsChartProperty.ConnectionString == "")
            {
                lblMsg.Text = "Connection String not null";
                return;
            }
            if (objclsChartProperty.ReportID == 0)
            {
                lblMsg.Text = "Report Id is not provided or is zero.";
                return;
            }
            else
            {
                ds.Reset();
                objclsChartProperty.Query = "select * from mReportMaster where ReportID='" + objclsChartProperty.ReportID + "'";
                ds = fillDs(objclsChartProperty);
                if (ds.Tables[0].Rows.Count == 0)
                {
                    lblMsg.Text = "No Record Found Check Report Id is Valid";
                    return;
                }
                if (ds.Tables[0].Rows[0]["ReportType"].ToString().Trim() != "Dashboard")
                {
                    lblMsg.Text = "Report Type is Valid";
                    return;
                }
                else
                {

                    objclsChartProperty.Query = "select sysDashboard.ChartType,sysDashboard.DataQuery,sysDashboard.[Text] ,sysDashboard.Value ,sysDashboard.LegentValue ,sysDashboard.Series,sysDashboard.Height,sysDashboard.Width ,sysDashboard.Is3D ,sysDashboard.IsLegend ,sysDashboard.LegentPosition ,sysDashboard.ChartBackgroundColor ,sysDashboard.ChartArea3DRotation,sysDashboard.ChartType,sysDashboard.ChartSyle,mReportMaster.Query,sysDashboard.XLable,sysDashboard.YLable ,sysDashboard.AllowPrint,sysDashboard.ChartBorderSkin,mReportMaster.ReportName,isNull(sysDashboard.ShowGridLineX,'true') as ShowGridLineX,isNull(sysDashboard.ShowGridLineY,'true') as ShowGridLineY from sysDashboard inner join mReportMaster on sysDashboard.D_ID=mReportMaster.ChartId and mReportMaster.ReportID='" + objclsChartProperty.ReportID + "'";
                    ds = fillDs(objclsChartProperty);
                    try
                    {
                        objclsChartProperty.Type = ds.Tables[0].Rows[0]["ChartType"].ToString();
                        objclsChartProperty.Text = ds.Tables[0].Rows[0]["Text"].ToString();
                        objclsChartProperty.Value = ds.Tables[0].Rows[0]["Value"].ToString();
                        objclsChartProperty.Series = Convert.ToBoolean(ds.Tables[0].Rows[0]["Series"].ToString());
                        objclsChartProperty.Height = Convert.ToInt32(ds.Tables[0].Rows[0]["Height"].ToString());
                        objclsChartProperty.Width = Convert.ToInt32(ds.Tables[0].Rows[0]["Width"].ToString());
                        objclsChartProperty.Is3D = Convert.ToBoolean(ds.Tables[0].Rows[0]["Is3D"].ToString());
                        objclsChartProperty.LegentValue = ds.Tables[0].Rows[0]["LegentValue"].ToString();
                        objclsChartProperty.IsLegend = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsLegend"].ToString());
                        objclsChartProperty.LegentPosition = ds.Tables[0].Rows[0]["LegentPosition"].ToString();
                        objclsChartProperty.ChartBackgroundColor = ds.Tables[0].Rows[0]["ChartBackgroundColor"].ToString();
                        objclsChartProperty.ChartArea3DRotation = Convert.ToInt32(ds.Tables[0].Rows[0]["ChartArea3DRotation"].ToString());
                        objclsChartProperty.ChartSyle = ds.Tables[0].Rows[0]["ChartSyle"].ToString();
                        objclsChartProperty.DataQuery = ds.Tables[0].Rows[0]["DataQuery"].ToString();
                        if (objclsChartProperty.IsQueryManualy == false)
                        {
                            objclsChartProperty.Query = objclsChartProperty.QueryParameter + ds.Tables[0].Rows[0]["Query"].ToString();
                        }
                        else
                        {
                            objclsChartProperty.Query = hfQuery.Value;
                        }
                        objclsChartProperty.Xlable = ds.Tables[0].Rows[0]["XLable"].ToString();
                        objclsChartProperty.Ylable = ds.Tables[0].Rows[0]["YLable"].ToString();
                        objclsChartProperty.AllowPrint = Convert.ToBoolean(ds.Tables[0].Rows[0]["AllowPrint"].ToString());
                        objclsChartProperty.ChartBorderSkin = ds.Tables[0].Rows[0]["ChartBorderSkin"].ToString();
                        objclsChartProperty.ReportName = ds.Tables[0].Rows[0]["ReportName"].ToString();
                        objclsChartProperty.ShowGridLineX = Convert.ToBoolean(ds.Tables[0].Rows[0]["ShowGridLineX"].ToString());
                        objclsChartProperty.ShowGridLineY = Convert.ToBoolean(ds.Tables[0].Rows[0]["ShowGridLineY"].ToString());
                    }
                    catch (Exception ex)
                    {
                        lblMsg.Text = ex.Message;
                    }
                }
            }

            SetChartProperty(objclsChartProperty);
            try
            {
                Chart1.DataBind();
            }
            catch (Exception ex)
            {
                lblMsg.Text = "Error on Chart Binding-" + ex.Message;
            }
            if (objclsChartProperty.ExportChart == true)
            {
                Exportchart(Chart1, objclsChartProperty);
            }


        }
        #endregion

        #region "Set Chart Property"
        private void SetChartProperty(ChartProperty objclsChartProperty)
        {
            ChartArea chartArea = new ChartArea();
            try
            {
                //------------------------------------------------------------------------------------------------------
                //     Button1 = new ImageButton();
                ImageButton1.Visible = objclsChartProperty.AllowPrint;

                Legend legend1 = new Legend();
                //// Set the Chart Properties

                Chart1.Width = (int)objclsChartProperty.Width;
                Chart1.Height = (int)objclsChartProperty.Height;


                Chart1.Palette = SetChartStyle(objclsChartProperty.ChartSyle);
                Chart1.BackColor = System.Drawing.Color.Transparent;
                Chart1.BorderSkin.SkinStyle = ChartBorderSkinStyle(objclsChartProperty);

                Chart1.BorderlineColor = System.Drawing.Color.AntiqueWhite;
                Chart1.BorderlineDashStyle = ChartDashStyle.Solid;
                Chart1.BorderlineWidth = 1;


                // Set the ChartArea properties
                Chart1.Titles.Add(new Title(objclsChartProperty.ReportName, Docking.Top, new System.Drawing.Font("Verdana", 8f, FontStyle.Bold), System.Drawing.Color.Black));
                chartArea.BackColor = System.Drawing.Color.Transparent;
                chartArea.Area3DStyle.Enable3D = objclsChartProperty.Is3D;
                if (objclsChartProperty.Is3D == true)
                {
                    chartArea.Area3DStyle.Rotation = objclsChartProperty.ChartArea3DRotation;
                    chartArea.Area3DStyle.IsClustered = true;

                }
                //// Add the ChartArea to the Chart

                Chart1.ChartAreas.Add(chartArea);
                Chart1.ChartAreas[0].AxisX.MajorGrid.Enabled = objclsChartProperty.ShowGridLineX;
                Chart1.ChartAreas[0].AxisY.MajorGrid.Enabled = objclsChartProperty.ShowGridLineY;
                if (objclsChartProperty.IsLegend == true)
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
                        legend1.Alignment = StringAlignment.Near;
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
        private void SetChartType(ChartProperty objclsChartProperty, ChartArea chartArea)
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
                    BindCustomeChart(objclsChartProperty, chartArea);
                    break;
            }
        }


        #endregion

        #region "Set Skin Back Color Style"
        public BorderSkinStyle ChartBorderSkinStyle(ChartProperty objclsChartProperty)
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

        #region "Add or Export PDF"

        public void ExportORAdd(ChartProperty objclsChartProperty)
        {

            if (objclsChartProperty.ExportChart == false)
            {
                // cpMain.Controls.Add(objclsChartProperty.ChartP);
            }
            else
            {
                Exportchart(objclsChartProperty.ChartP, objclsChartProperty);
            }
        }

        #endregion

        #region "Coloum Chart :Binding Method for Coloum Chart "

        private void BindColumnChart(ChartProperty objclsChartProperty, ChartArea chartArea)
        {
            ds = fillDs(objclsChartProperty);
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
                        Chart1.BackColor = SetBGColor(objclsChartProperty);
                        Chart1.ChartAreas[0].AxisX.Interval = 1;
                        Chart1.ChartAreas[0].AxisX.TextOrientation = TextOrientation.Auto;
                        Chart1.ChartAreas[0].AxisX.Title = objclsChartProperty.Xlable;

                        Double temp = 0;
                        if (list.Max() > objclsChartProperty.Height)
                        {
                            temp = list.Max() / objclsChartProperty.Height;
                            temp = list.Max() / x;
                            if (x > temp || x == 1)
                            { x = temp; }
                        }
                        else
                        {

                            if (list.Max() != 1)
                            {
                                temp = objclsChartProperty.Height / list.Max();
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
                        Chart1.ChartAreas[0].AxisY.Title = objclsChartProperty.Ylable;
                        Chart1.Series.Add(series);

                        Axis yAxis = new Axis(chartArea, AxisName.Y);
                        Axis xAxis = new Axis(chartArea, AxisName.X);

                        // Bind the data to the chart
                        Chart1.Series[ds.Tables[_intdsTcount].Rows[0][objclsChartProperty.LegentValue].ToString()].Points.DataBindXY(xValues, yValues);
                        Chart1.Series[ds.Tables[_intdsTcount].Rows[0][objclsChartProperty.LegentValue].ToString()].ToolTip = "" + objclsChartProperty.Xlable + " \t= #VALX\n" + ds.Tables[_intdsTcount].Rows[0][objclsChartProperty.LegentValue].ToString() + "  " + objclsChartProperty.Ylable + "\t= #VALY";
                    }
                }
                Chart1.DataBind();
            }
            catch (Exception ex)
            {
                lblMsg.Text = "Error on Coloum Chart Binding-" + ex.Message;
            }

            BindGrid(objclsChartProperty);
        }
        
        #endregion

        #region "Pie Chart : Binding Method for pie chart"
        private void BindPieChart(ChartProperty objclsChartProperty, ChartArea chartArea)
        {
            ds = fillDs(objclsChartProperty);
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
                    Chart1.BackColor = SetBGColor(objclsChartProperty);
                    Chart1.Series.Add(series);

                    Axis yAxis = new Axis(chartArea, AxisName.Y);
                    Axis xAxis = new Axis(chartArea, AxisName.X);

                    // Bind the data to the chart
                    Chart1.Series[ds.Tables[0].Rows[0][objclsChartProperty.LegentValue].ToString()].Points.DataBindXY(xValues, yValues);
                    Chart1.Series[ds.Tables[0].Rows[0][objclsChartProperty.LegentValue].ToString()].ToolTip = "" + objclsChartProperty.Xlable + " \t= #VALX\n" + objclsChartProperty.Ylable + "\t= #VALY";

                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = "Error on Pie Chart Binding-" + ex.Message;
            }

            BindGrid(objclsChartProperty);
        }
        #endregion

        #region "Funnel Chart : Binding Method for funnel chart"
        private void BindFunnelChart(ChartProperty objclsChartProperty, ChartArea chartArea)
        {
            ds = fillDs(objclsChartProperty);
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
                    Chart1.BackColor = SetBGColor(objclsChartProperty);

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
                    Chart1.Series["Default"].ToolTip = "" + objclsChartProperty.Xlable + " \t= #VALX\n" + objclsChartProperty.Ylable + "\t= #VALY";
                    BindGrid(objclsChartProperty);
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = "Error on Funnel Chart Binding-" + ex.Message;
            }

        }
        #endregion

        #region "Line Chart :Binding Method for Line Chart "
        private void BindLineChart(ChartProperty objclsChartProperty, ChartArea chartArea)
        {
            ds = fillDs(objclsChartProperty);
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
                        Chart1.BackColor = SetBGColor(objclsChartProperty);
                        series.IsValueShownAsLabel = true;
                        Chart1.ChartAreas[0].AxisX.Interval = 1;
                        Chart1.ChartAreas[0].AxisX.TextOrientation = TextOrientation.Auto;
                        Chart1.ChartAreas[0].AxisX.Title = objclsChartProperty.Xlable;
                        Chart1.ChartAreas[0].AxisY.Interval = 1;
                        Chart1.ChartAreas[0].AxisY.TextOrientation = TextOrientation.Auto;
                        Chart1.ChartAreas[0].AxisY.Title = objclsChartProperty.Ylable;
                        series.BorderWidth = 5;
                        Chart1.Series.Add(series);

                        Axis yAxis = new Axis(chartArea, AxisName.Y);
                        Axis xAxis = new Axis(chartArea, AxisName.X);

                        // Bind the data to the chart
                        Chart1.Series[ds.Tables[_intdsTcount].Rows[0][objclsChartProperty.LegentValue].ToString()].Points.DataBindXY(xValues, yValues);
                        Chart1.Series[ds.Tables[_intdsTcount].Rows[0][objclsChartProperty.LegentValue].ToString()].ToolTip = "" + objclsChartProperty.Xlable + " \t= #VALX\n" + ds.Tables[_intdsTcount].Rows[0][objclsChartProperty.LegentValue].ToString() + "  " + objclsChartProperty.Ylable + "\t= #VALY";
                    }
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = "Error on Line Chart Binding-" + ex.Message;
            }

            BindGrid(objclsChartProperty);

        }
        #endregion

        #region "Stock Column Chart :Bind Stock Column/Bar Chart"
        private void BindStackedColumn(ChartProperty objclsChartProperty, ChartArea chartArea)
        {
            Double x = 1;
            ds = fillDs(objclsChartProperty);
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
                        Chart1.BackColor = SetBGColor(objclsChartProperty);
                        Chart1.ChartAreas[0].AxisX.Interval = 1;
                        Chart1.ChartAreas[0].AxisX.TextOrientation = TextOrientation.Auto;
                        Chart1.ChartAreas[0].AxisX.Title = objclsChartProperty.Xlable;

                        Double temp = 0;
                        if (list.Max() > objclsChartProperty.Height)
                        {
                            temp = list.Max() / objclsChartProperty.Height;
                            temp = list.Max() / x;
                            if (x > temp || x == 1)
                            { x = temp; }
                        }
                        else
                        {

                            if (list.Max() != 1)
                            {
                                if (list.Max() > objclsChartProperty.Height)
                                {
                                    temp = objclsChartProperty.Height / list.Max();
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
                        Chart1.ChartAreas[0].AxisY.Title = objclsChartProperty.Ylable;
                        Chart1.Series.Add(series);

                        Axis yAxis = new Axis(chartArea, AxisName.Y);
                        Axis xAxis = new Axis(chartArea, AxisName.X);

                        // Bind the data to the chart
                        Chart1.Series[ds.Tables[_intdsTcount].Rows[0][objclsChartProperty.LegentValue].ToString()].Points.DataBindXY(xValues, yValues);
                        Chart1.Series[ds.Tables[_intdsTcount].Rows[0][objclsChartProperty.LegentValue].ToString()].ToolTip = "" + objclsChartProperty.Xlable + " \t= #VALX\n" + ds.Tables[_intdsTcount].Rows[0][objclsChartProperty.LegentValue].ToString() + "  " + objclsChartProperty.Ylable + "\t= #VALY";
                    }

                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = "Error on Stock Chart Binding-" + ex.Message;
            }

            BindGrid(objclsChartProperty);


        }
        #endregion

        #region "Range Bar Chart :Bind Range colume/Bar Chart"
        private void BindRangeBarChart(ChartProperty objclsChartProperty, ChartArea chartArea)
        {
            double x = 0;
            ds = fillDs(objclsChartProperty);
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
                        Chart1.BackColor = SetBGColor(objclsChartProperty);
                        Chart1.ChartAreas[0].AxisX.Interval = 1;
                        Chart1.ChartAreas[0].AxisX.TextOrientation = TextOrientation.Auto;
                        Chart1.ChartAreas[0].AxisX.Title = objclsChartProperty.Xlable;
                        Double temp = 0;
                        if (list.Max() > objclsChartProperty.Height)
                        {
                            temp = list.Max() / objclsChartProperty.Height;
                            temp = list.Max() / x;
                            if (x > temp || x == 1)
                            { x = temp; }
                        }
                        else
                        {

                            if (list.Max() != 1)
                            {
                                temp = objclsChartProperty.Height / list.Max();
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
                        Chart1.ChartAreas[0].AxisY.Title = objclsChartProperty.Ylable;
                        Chart1.Series.Add(series);
                        if (_intdsTcount == 0) { Chart1.Series[0].CustomProperties = "PointWidth=0.7"; }
                        if (_intdsTcount >= 1) { Chart1.Series[_intdsTcount].CustomProperties = "PointWidth=0.4, DrawSideBySide=false"; }

                        Axis yAxis = new Axis(chartArea, AxisName.Y);
                        Axis xAxis = new Axis(chartArea, AxisName.X);

                        // Bind the data to the chart
                        Chart1.Series[ds.Tables[_intdsTcount].Rows[0][objclsChartProperty.LegentValue].ToString()].Points.DataBindXY(xValues, yValues);
                        Chart1.Series[ds.Tables[_intdsTcount].Rows[0][objclsChartProperty.LegentValue].ToString()].ToolTip = "" + objclsChartProperty.Xlable + " \t= #VALX\n" + ds.Tables[_intdsTcount].Rows[0][objclsChartProperty.LegentValue].ToString() + "  " + objclsChartProperty.Ylable + "\t= #VALY";
                    }
                }
                if (objclsChartProperty.Height < ds.Tables[0].Rows.Count * 30) { Chart1.Height = ds.Tables[0].Rows.Count * 30; }
            }
            catch (Exception ex)
            {
                lblMsg.Text = "Error on Renage Chart Binding-" + ex.Message;
            }

            BindGrid(objclsChartProperty);


        }
        #endregion

        #region "Date wise Project status renge chart (Only for excel gas)"
        public void BindProjectStatusChart(ChartProperty objclsChartProperty, ChartArea chartArea)
        {
            DataSet ds = new DataSet();
            ds = fillDs(objclsChartProperty);
            //DateTime dtMinDate=ds.Tables[0].Select( 
            try
            {
                List<DateTime> MinDate = ds.Tables[0].AsEnumerable().Select(al => al.Field<DateTime>("PstartDate")).Distinct().ToList();
                DateTime min = MinDate.Min();
                List<DateTime> MaxDate = ds.Tables[0].AsEnumerable().Select(al => al.Field<DateTime>("PendDate")).Distinct().ToList();
                DateTime max = MaxDate.Max();    //var mindate = (from dt in ds.Tables[0].AsEnumerable())

                double dToday = 0;
                double dStartDate = DateTime.Today.ToOADate();

                TimeSpan tm = new TimeSpan();
                tm = System.DateTime.Now - min;
                if (tm.Days > 0)
                {
                    Chart1.ChartAreas[0].AxisY.Minimum = dStartDate - min.ToOADate();
                }
                else
                {
                    Chart1.ChartAreas[0].AxisY.Minimum = dStartDate + 7;
                }

                //Chart1.ChartAreas[0].AxisY.Minimum = dStartDate;
                Chart1.ChartAreas[0].AxisY.LabelStyle.Interval = 1;
                Chart1.ChartAreas[0].AxisY.LabelStyle.IntervalType = DateTimeIntervalType.Days;
                Chart1.ChartAreas[0].AxisY.IsLabelAutoFit = true;


                List<double> list = new List<double>();
                String sx = "";
                String pName = "";
                for (int i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
                {
                    list.Add(Chart1.ChartAreas[0].AxisY.Minimum + Convert.ToDateTime(ds.Tables[0].Rows[i]["PendDate"].ToString()).ToOADate());
                    tm = Convert.ToDateTime(ds.Tables[0].Rows[i]["PstartDate"].ToString()) - Convert.ToDateTime(ds.Tables[0].Rows[i]["PendDate"].ToString());
                    if (i == 0)
                    {
                        pName = pName + ds.Tables[0].Rows[i][objclsChartProperty.Text].ToString();
                        sx = sx + tm.Days.ToString();
                    }
                    else
                    {
                        pName = pName + "," + ds.Tables[0].Rows[i][objclsChartProperty.Text].ToString();
                        sx = sx + "," + tm.Days.ToString();
                    }
                }

                string[] task = pName.Split(',');

                double[] start = list.ToArray();
                String[] end = sx.Split(',');


                Series ser = new Series();
                ser.YValuesPerPoint = 2;
                ser.ChartType = SeriesChartType.RangeBar;
                Chart1.Series.Add(ser);
                int pos = 1;
                string lastValue = "";
                for (int i = 0; i < start.Length - 1; i++)
                {

                    string xValue = task[i];
                    if (lastValue != xValue)
                        pos++;

                    string yValues = (dStartDate + Convert.ToInt32(start[i])).ToString() + "," + (dStartDate + Convert.ToInt32(end[i])).ToString();
                    DataPoint pt = new DataPoint(pos, yValues);
                    pt.AxisLabel = xValue;
                    ser.Points.Add(pt);

                    lastValue = xValue;
                }


                double[] actualStart = start;
                String[] actualEnd = end;
                Series ser2 = new Series();
                ser2.YValuesPerPoint = 2;
                ser2.ChartType = SeriesChartType.RangeBar;
                Chart1.Series.Add(ser2);
                pos = 1;
                lastValue = "";
                for (int i = 0; i < start.Length - 1; i++)
                {
                    string xValue = task[i];
                    if (lastValue != xValue)
                        pos++;

                    string yValues = (dStartDate + Convert.ToInt32(actualStart[i])).ToString() + "," + (dStartDate + Convert.ToInt32(actualEnd[i])).ToString();
                    DataPoint pt = new DataPoint(pos, yValues);
                    pt.AxisLabel = xValue;
                    ser2.Points.Add(pt);

                    if (dStartDate + dToday > actualStart[i])
                    {
                        if (start[i] < actualStart[i] || Convert.ToInt32(end[i]) < Convert.ToInt32(actualEnd[i]))
                            pt.Color = System.Drawing.Color.Red;
                        else if (dStartDate + dToday < Convert.ToInt32(end[i]))
                            pt.Color = System.Drawing.Color.Lime;
                        else if (end[i] == actualEnd[i])
                            pt.Color = System.Drawing.Color.Gray;
                    }

                    lastValue = xValue;
                }

                StripLine stripLine = new StripLine();
                stripLine.StripWidth = 1;
                stripLine.Font = new System.Drawing.Font("Arial", 8.25F, FontStyle.Bold);
                stripLine.ForeColor = System.Drawing.Color.Gray;
                stripLine.Text = "Today";
                stripLine.TextOrientation = TextOrientation.Rotated90;
                stripLine.BorderColor = System.Drawing.Color.Black;
                stripLine.BackColor = System.Drawing.Color.PaleTurquoise;
                stripLine.IntervalOffset = dStartDate + dToday;
                stripLine.TextAlignment = StringAlignment.Center;
                stripLine.TextLineAlignment = StringAlignment.Near;

                Chart1.ChartAreas[0].AxisY.StripLines.Add(stripLine);

                LegendItem legendItem = new LegendItem();
                legendItem.Color = System.Drawing.Color.Red;
                legendItem.Name = "Late";
                Chart1.Legends[0].CustomItems.Add(legendItem);

                foreach (DataPoint pt in Chart1.Series[1].Points)
                {
                    if (pt.YValues[0] == pt.YValues[1])
                        pt.Color = System.Drawing.Color.Transparent;
                }

                foreach (DataPoint pt in Chart1.Series[0].Points)
                {
                    if (pt.YValues[0] == pt.YValues[1])
                        pt.Color = System.Drawing.Color.Transparent;
                }
                Chart1.Series[0].CustomProperties = "PointWidth=0.7";
                Chart1.Series[1].CustomProperties = "PointWidth=0.2, DrawSideBySide=false";
            }
            catch (Exception ex)
            {
                lblMsg.Text = "Error on Renage Chart for excel gas Binding-" + ex.Message;
            }

        }
        #endregion

        #region "Custome Chart :Binding Method for Coloum Chart "

        private void BindCustomeChart(ChartProperty objclsChartProperty, ChartArea chartArea)
        {
            ds = fillDs(objclsChartProperty);
            try
            {
                for (int _intdsTcount = 0; _intdsTcount <= ds.Tables.Count - 1; _intdsTcount++)
                {
                    if (ds.Tables[_intdsTcount].Rows.Count < 0)
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
                        Chart1.BackColor = SetBGColor(objclsChartProperty);
                        Chart1.ChartAreas[0].AxisX.Interval = 1;
                        Chart1.ChartAreas[0].AxisX.TextOrientation = TextOrientation.Auto;
                        Chart1.ChartAreas[0].AxisX.Title = objclsChartProperty.Xlable;
                        Chart1.ChartAreas[0].AxisY.Interval = 1;
                        Chart1.ChartAreas[0].AxisY.TextOrientation = TextOrientation.Auto;
                        Chart1.ChartAreas[0].AxisY.Title = objclsChartProperty.Ylable;
                        Chart1.Series.Add(series);

                        Axis yAxis = new Axis(chartArea, AxisName.Y);
                        Axis xAxis = new Axis(chartArea, AxisName.X);

                        // Bind the data to the chart
                        Chart1.Series[ds.Tables[_intdsTcount].Rows[0][objclsChartProperty.LegentValue].ToString()].Points.DataBindXY(xValues, yValues);
                        Chart1.Series[ds.Tables[_intdsTcount].Rows[0][objclsChartProperty.LegentValue].ToString()].ToolTip = "" + objclsChartProperty.Xlable + " \t= #VALX\n" + ds.Tables[_intdsTcount].Rows[0][objclsChartProperty.LegentValue].ToString() + "  " + objclsChartProperty.Ylable + "\t= #VALY";
                    }
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = "Error on Custom Chart Binding-" + ex.Message;
            }

            BindGrid(objclsChartProperty);
        }


        #endregion

        #region "Set Back Color"
        public System.Drawing.Color SetBGColor(ChartProperty objclsChartProperty)
        {
            switch (objclsChartProperty.ChartBackgroundColor.ToLower())
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

        #region "Common Method"
        public DataSet fillDs(ChartProperty objReportProperty)
        {
            DataSet ds = new DataSet();
            SqlDataAdapter adp = new SqlDataAdapter();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = objReportProperty.ConnectionString;
            try
            {
                con.Open();
                adp = new SqlDataAdapter(objReportProperty.Query, con);
                ds.Reset();
                adp.Fill(ds);
            }
            catch (SqlException ex)
            {
               lblMsg.Text="Query Error - "+ex.Message;
            }

            finally
            {
                con.Close();
            }

            return ds;
        }
        #endregion

        #region "Export To PDF Logic"
        void Exportchart(Chart chart, ChartProperty objclsChartProperty)
        {
            String s = System.Guid.NewGuid().ToString();
            chart.SaveImage(Server.MapPath("~/chart_" + s + ".png"), ChartImageFormat.Png);
            iTextSharp.text.Rectangle r = new iTextSharp.text.Rectangle(Convert.ToInt32(Chart1.Width.Value) + 10, Convert.ToInt32(Chart1.Height.Value) + 10);

            iTextSharp.text.Document document = new iTextSharp.text.Document(r, 5, 5, 5, 5);
            MemoryStream msReport = new MemoryStream();

            try
            {

                iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(document, msReport);
                document.AddAuthor("Test");
                document.AddSubject("Export to PDF");
                document.Open();
                iTextSharp.text.Chunk c = new iTextSharp.text.Chunk("Export chart to PDF", iTextSharp.text.FontFactory.GetFont("VERDANA", 15));
                iTextSharp.text.Paragraph p = new iTextSharp.text.Paragraph();
                p.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
                iTextSharp.text.Image hImage;
                hImage = iTextSharp.text.Image.GetInstance(Server.MapPath("~/chart_" + s + ".png"));

                float NewWidth = (float)Chart1.Width.Value + 10;
                float MaxHeight = (float)Chart1.Height.Value + 10;

                hImage.ScaleAbsolute(NewWidth, MaxHeight);
                document.Add(p);
                document.Add(hImage);
                document.Close();

                Response.AddHeader("Content-type", "application/pdf");
                Response.AddHeader("Content-Disposition", "attachment; filename=chart.pdf");
                Response.OutputStream.Write(msReport.GetBuffer(), 0, msReport.GetBuffer().Length);
                File.Delete(Server.MapPath("~/chart_" + s + ".png"));

            }
            catch (System.Threading.ThreadAbortException ex)
            {
                throw new Exception("Error occured: " + ex);
            }
            catch (Exception ex1)
            {
                lblMsg.Text = "Error on Export PDF (0L)-" + ex1.Message;
            }

        }
        #endregion

        #region "Export To PDF"
        public void ExportPDF(ChartProperty objclsChartProperty)
        {
            objclsChartProperty.ExportChart = true;
            BindChart(objclsChartProperty);

        }
        #endregion

        #region "Page event"

        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            ChartProperty objclsChartProperty = new ChartProperty();
            try
            {
                objclsChartProperty.ReportID = Convert.ToInt32(hfRepeortid.Value);
                objclsChartProperty.QueryParameter = hfQueryParameter.Value;
                objclsChartProperty.ConnectionString = hfConnectionString.Value;
                objclsChartProperty.IsQueryManualy = Convert.ToBoolean(hfQueryFlag.Value);
            }
            catch (Exception ex)
            {
                lblMsg.Text = "Error on Export Pdf Click -" + ex.Message;
            }

            ExportPDF(objclsChartProperty);
        }


        protected override void OnInit(EventArgs e)
        {
            if (!IsPostBack)
            {
                this.ImageButton1.Click += new ImageClickEventHandler(ImageButton1_Click);
            }

        }

        private void BindGrid(ChartProperty objclsChartProperty)
        {

            if (objclsChartProperty.DataQuery != "")
            {
                objclsChartProperty.Query = objclsChartProperty.QueryParameter + objclsChartProperty.DataQuery;
                ds = fillDs(objclsChartProperty);
                try
                {


                    GV_DashData.DataSource = ds;
                    GV_DashData.DataBind();
                    

                }
                catch (Exception ex)
                {
                    lblMsg.Text = "Error on Grid Data Binding-" + ex.Message;
                }

            }
        }

        public void exporttoexcel()
        {
            try
            {
                Response.Clear();
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = "application/vnd.xls";
                System.IO.StringWriter stringWrite = new System.IO.StringWriter();
                System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
               GV_DashData.RenderControl(htmlWrite);
                Response.Write(stringWrite.ToString());
                Response.End();
            }
            catch (Exception ex)
            {
                lblMsg.Text = "Error on Export To Excel -" + ex.Message;
            }


        }



        protected void ImageButton2_Click(object sender, ImageClickEventArgs e)
        {
            exporttoexcel();
        }
        #endregion

        #region "Bind From Object details"


        #endregion

    }

    public class ChartProperty
    {
        #region "Property Variable"
        private Int32 _intReportID;
        private String _strConnectionString;
        private Int32 _intD_ID;
        private String _strChartType;
        private String _strText;
        private String _strValue;
        private String _strLegentValue;
        private Boolean _bolSeries;
        private Int32 _intHeight;
        private Int32 _intWidth;
        private Boolean _bolIs3D;
        private Boolean _bolIsLegend;
        private String _strLegentPosition;
        private String _strChartBackgroundColor;
        private Int32 _strChartArea3DRotation;
        private String _strChartSyle;
        private String _strType;
        private String _strAllowEdit;
        private String _strQuery;
        private String _strXlable;
        private String _strYlable;
        private String _strQueryParameter;
        private String _strChartBorderSkin;
        private Chart _chartP;
        private Boolean _BolExportChart;
        private Boolean _bolAllowPrint;
        private String _strReportName;
        private Boolean _bolIsQueryManualy;
        private String _strQueryData;
        private Boolean _bolGridLineX;
        private Boolean _bolGridLineY;

        #endregion

        #region "Property Declaration"
        public Int32 ReportID
        {
            get { return _intReportID; }
            set { _intReportID = value; }
        }
        public String ConnectionString
        {
            get { return _strConnectionString; }
            set { _strConnectionString = value; }
        }
        public String ChartType
        {
            get { return _strChartType; }
            set { _strChartType = value; }
        }
        public String ChartBorderSkin
        {
            get { return _strChartBorderSkin; }
            set { _strChartBorderSkin = value; }
        }
        public String Text
        {
            get { return _strText; }
            set { _strText = value; }
        }
        public String Value
        {
            get { return _strValue; }
            set { _strValue = value; }
        }
        public String LegentValue
        {
            get { return _strLegentValue; }
            set { _strLegentValue = value; }
        }
        public String ReportName
        {
            get { return _strReportName; }
            set { _strReportName = value; }
        }
        public Boolean Series
        {
            get { return _bolSeries; }
            set { _bolSeries = value; }
        }
        public Int32 Height
        {
            get { return _intHeight; }
            set { _intHeight = value; }
        }
        public Int32 Width
        {
            get { return _intWidth; }
            set { _intWidth = value; }
        }
        public Boolean Is3D
        {
            get { return _bolIs3D; }
            set { _bolIs3D = value; }
        }
        public Boolean IsLegend
        {
            get { return _bolIsLegend; }
            set { _bolIsLegend = value; }
        }
        public String LegentPosition
        {
            get { return _strLegentPosition; }
            set { _strLegentPosition = value; }
        }
        public String ChartBackgroundColor
        {
            get { return _strChartBackgroundColor; }
            set { _strChartBackgroundColor = value; }
        }
        public Int32 ChartArea3DRotation
        {
            get { return _strChartArea3DRotation; }
            set { _strChartArea3DRotation = value; }
        }
        public String ChartSyle
        {
            get { return _strChartSyle; }
            set { _strChartSyle = value; }
        }
        public String Type
        {
            get { return _strType; }
            set { _strType = value; }
        }
        public String AllowEdit
        {
            get { return _strAllowEdit; }
            set { _strAllowEdit = value; }
        }
        public String Query
        {
            get { return _strQuery; }
            set { _strQuery = value; }
        }
        public String QueryParameter
        {
            get { return _strQueryParameter; }
            set { _strQueryParameter = value; }
        }
        public String Xlable
        {
            get { return _strXlable; }
            set { _strXlable = value; }
        }
        public String Ylable
        {
            get { return _strYlable; }
            set { _strYlable = value; }
        }
        public Chart ChartP
        {
            get { return _chartP; }
            set { _chartP = value; }
        }
        public Boolean ExportChart
        {
            get { return _BolExportChart; }
            set { _BolExportChart = value; }
        }
        public Boolean AllowPrint
        {
            get { return _bolAllowPrint; }
            set { _bolAllowPrint = value; }
        }

        public Boolean IsQueryManualy
        {
            get { return _bolIsQueryManualy; }
            set { _bolIsQueryManualy = value; }
        }
        public string DataQuery { get { return _strQueryData; } set { _strQueryData = value; } }

        public Boolean ShowGridLineY
        {
            get { return _bolGridLineY; }
            set { _bolGridLineY = value; }
        }
        public Boolean ShowGridLineX
        {
            get { return _bolGridLineX; }
            set { _bolGridLineX = value; }
        }
        #endregion

        public ChartProperty()
        {
            IsQueryManualy = false;
            ExportChart = false;
        }


    }

}