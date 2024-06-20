using FastReport;
using System.Collections;

namespace DesignFormGS
{
    public partial class DesignFormPMR : Form
    {
        private Report loReport;
        public DesignFormPMR()
        {
            InitializeComponent();
        }

        private void DesignFormPMR_Load(object sender, EventArgs e)
        {
            loReport = new Report();
        }

        private void PMR01001_Click(object sender, EventArgs e)
        {
            ArrayList loData = new ArrayList();
            loData.Add(PMR01000Common.Model.PMR01001DummyData.DefaultDataWithHeader());
            loReport.RegisterData(loData, "ResponseDataModel");
            loReport.Design();
        }

        private void PMR01002_Click(object sender, EventArgs e)
        {
            ArrayList loData = new ArrayList();
            loData.Add(PMR01000Common.Model.PMR01002DummyData.DefaultDataWithHeader());
            loReport.RegisterData(loData, "ResponseDataModel");
            loReport.Design();
        }

        private void PMR01003_Click(object sender, EventArgs e)
        {
            ArrayList loData = new ArrayList();
            loData.Add(PMR01000Common.Model.PMR01003DummyData.DefaultDataWithHeader());
            loReport.RegisterData(loData, "ResponseDataModel");
            loReport.Design();
        }

        private void PMR02200_Click(object sender, EventArgs e)
        {
            ArrayList loData = new ArrayList();
            loData.Add(PMR02200Common.Model.PMR02200DummyData.DefaultDataWithHeader());
            loReport.RegisterData(loData, "ResponseDataModel");
            loReport.Design();
        }

      
        private void PMR02101SummaryButton_Click(object sender, EventArgs e)
        {
            ArrayList loData = new ArrayList();
            loData.Add(PMR02200Common.Model.PMR02101SummaryDummyData.DefaultDataWithHeader());
            loReport.RegisterData(loData, "ResponseDataModel");
            loReport.Design();
        }

        private void PMR02102SummaryButton_Click(object sender, EventArgs e)
        {
            ArrayList loData = new ArrayList();
            loData.Add(PMR02200Common.Model.PMR02102SummaryDummyData.DefaultDataWithHeader());
            loReport.RegisterData(loData, "ResponseDataModel");
            loReport.Design();
        }


        private void PMR02103SummaryButton_Click(object sender, EventArgs e)
        {
            ArrayList loData = new ArrayList();
            loData.Add(PMR02200Common.Model.PMR02103SummaryDummyData.DefaultDataWithHeader());
            loReport.RegisterData(loData, "ResponseDataModel");
            loReport.Design();
        }

        private void PMR02104SummaryButton_Click(object sender, EventArgs e)
        {
            ArrayList loData = new ArrayList();
            loData.Add(PMR02200Common.Model.PMR02104SummaryDummyData.DefaultDataWithHeader());
            loReport.RegisterData(loData, "ResponseDataModel");
            loReport.Design();
        }
    }
}