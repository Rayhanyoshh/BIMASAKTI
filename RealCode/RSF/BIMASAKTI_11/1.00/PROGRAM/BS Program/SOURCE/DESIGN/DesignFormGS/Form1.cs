using FastReport;
using System.Collections;

namespace DesignFormGS
{
    public partial class DesignFormGS : Form
    {
        private Report loReport;
        public DesignFormGS()
        {
            InitializeComponent();
        }

        private void DesignFormGS_Load(object sender, EventArgs e)
        {
            loReport = new Report();
        }

        private void GSM01000_Click(object sender, EventArgs e)
        {
            ArrayList loData = new ArrayList();
            loData.Add(GSM01000Common.Model.GSM01000ModelDummyData.DefaultDataWithHeader());
            loReport.RegisterData(loData, "ResponseDataModel");
            loReport.Design();
        }

        private void GSM01000GOA_Click(object sender, EventArgs e)
        {
            ArrayList loData = new ArrayList();
            loData.Add(GSM001000Common.Model.GSM01000GOAModelDummyData.DefaultDataWithHeader());
            loReport.RegisterData(loData, "ResponseDataModel");
            loReport.Design();
        }

        private void GSM08500_Click(object sender, EventArgs e)
        {
            ArrayList loData = new ArrayList();
            loData.Add(GSM008500Common.Model.GSM08500ModelDummyData.DefaultDataWithHeader());
            loReport.RegisterData(loData, "ResponseDataModel");
            loReport.Design();
        }
    }
}