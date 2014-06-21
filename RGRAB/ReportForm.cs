﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Finisar.SQLite;


namespace RGRAB
{
    public partial class ReportForm : Form
    {
        public ReportForm()
        {
            InitializeComponent();
        }
        int itemCount = 0;

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void rptConsumptionReport_Click(object sender, EventArgs e)
        {
            ConsumptionPrint();
        }

        public void ConsumptionPrint()
        {

            PrintDocument pdoc = new PrintDocument();
            PrintDialog pd = new PrintDialog();
            PrinterSettings ps = new PrinterSettings();

            pd.Document = pdoc;
            pdoc.DefaultPageSettings.Margins.Bottom = 0;
            pdoc.DefaultPageSettings.Margins.Left = 0;
            pdoc.DefaultPageSettings.Margins.Right = 0;
            pdoc.DefaultPageSettings.Margins.Top = 0;

            pdoc.PrintPage += new PrintPageEventHandler(pdoc_PrintPage);

            DialogResult result = pd.ShowDialog();
            if (result == DialogResult.OK)
            {
                PrintPreviewDialog pp = new PrintPreviewDialog();
                pp.Document = pdoc;
                result = pp.ShowDialog();
                if (result == DialogResult.OK)
                {
                pdoc.Print();
                }
            }
        }

        void pdoc_PrintPage(object sender, PrintPageEventArgs e)
        {

            //string valueMonth = subBatchMonth.Text;
            string currentYear = DateTime.Now.Year.ToString();
            string underLine = "-------------------------------------------------------------------";

            int itemCounter = 0;

            Graphics graphics = e.Graphics;
            Font font = new Font("Courier New", 10);
            SolidBrush brush = new SolidBrush(Color.Black);
            e.PageSettings.PaperSize = new PaperSize("A4", 850, 1100);
            float pageWidth = e.PageSettings.PrintableArea.Width;
            float pageHeight = e.PageSettings.PrintableArea.Height;

            //float fontHeight = font.GetHeight();

            int startX = 100;
            int startY = 25;
            int OffsetY = 20;
            DateTime today = DateTime.Today;
            string Today = today.ToString("MM/dd/yyyy"); // As String

            graphics.DrawString("Mont Vert Seville CHS Gas Consumption Report", new Font("Courier New", 16, FontStyle.Bold), brush, startX, startY + OffsetY);

            OffsetY = OffsetY + 30;
            graphics.DrawString("Date:" + Today, font, brush, startX, startY + OffsetY);

            OffsetY = OffsetY + 10;
            graphics.DrawString(underLine, font, brush, startX, startY + OffsetY);

            OffsetY = OffsetY + 30;
            graphics.DrawString("Flat No", new Font("Courier New", 12, FontStyle.Bold), brush, startX, startY + OffsetY);
            graphics.DrawString("Resident Name", new Font("Courier New", 12, FontStyle.Bold), brush, (startX + 170), startY + OffsetY);
            graphics.DrawString("Consumption (Cylinder Units)", new Font("Courier New", 12, FontStyle.Bold), brush, (startX + 400), startY + OffsetY);

            FirstLoad fstLoad = new FirstLoad();
            List<ConsumptionDetail> consumptionList = fstLoad.getConsumption();
            ConsumptionDetail consumptionDet;

            for (int i = itemCount; i < consumptionList.Count; i++) // Loop through List with for
            {
                if (itemCount < consumptionList.Count)
                {
                    consumptionDet = consumptionList[i];

                    string flatNo = consumptionDet.FlatNo;
                    string residentName = consumptionDet.Name;
                    string consumption = consumptionDet.Consumption;

                    OffsetY = OffsetY + 30;
                    graphics.DrawString(flatNo, font, brush, (startX + 10), startY + OffsetY);
                    graphics.DrawString(residentName, font, brush, (startX + 150), startY + OffsetY);
                    graphics.DrawString(consumption, font, brush, (startX + 475), startY + OffsetY);

                    OffsetY = OffsetY + 15;

                    itemCounter++;
                    itemCount++;

                    if ((itemCounter == 50) && (itemCount < consumptionList.Count))
                    {
                        e.HasMorePages = true;
                        return;
                    }
                    else
                    {
                        e.HasMorePages = false;
                    }

                }
            }
        }

        private void rptDefaulterReport_Click(object sender, EventArgs e)
        {
            DefaulterReportForm defaultForm = new DefaulterReportForm();
            defaultForm.Show();
        }
    }
}
