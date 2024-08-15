using FastReport;
using System.Collections;

namespace DesignFormGS
{
    public partial class DesignFormTX : Form
    {
        private Report loReport;
        public DesignFormTX()
        {
            InitializeComponent();
        }

        private void DesignFormPMR_Load(object sender, EventArgs e)
        {
            loReport = new Report();
        }


        private void TXR00100_Click_1(object sender, EventArgs e)
        {
            ArrayList loData = new ArrayList();
            loData.Add(TXR00100Common.Model.TXR00100DummyData.DefaultDataWithHeader());
            loReport.RegisterData(loData, "ResponseDataModel");
            loReport.Design();
        }
    }
}