using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormTMCX : Form
    {
        private System.Data.SqlClient.SqlConnection sqlConn = new System.Data.SqlClient.SqlConnection();
        private System.Data.SqlClient.SqlCommand sqlComm = new System.Data.SqlClient.SqlCommand();
        private System.Data.SqlClient.SqlDataReader sqldr;
        private System.Data.SqlClient.SqlDataAdapter sqlDA = new System.Data.SqlClient.SqlDataAdapter();
        private System.Data.DataSet dSet = new DataSet();

        public string strConn = "";

        public int intUserID = 0;
        public int intUserLimit = 0;
        public string strUserLimit = "";
        public string strUserName = "";
        public int intUserBM = 0;

        private string strDJBH = "";
        private int intDJID = 0;
        private string sDJClass = "";

        private bool isSaved = false;
        private ClassGetInformation cGetInformation;
        
        public FormTMCX()
        {
            InitializeComponent();
        }

        private void FormTMCX_Load(object sender, EventArgs e)
        {
            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;
            cGetInformation = new ClassGetInformation(strConn);
        }

        private void btnTM_Click(object sender, EventArgs e)
        {
            this.textBoxTM.Focus();
            this.textBoxTM.SelectAll();
        }

        private void textBoxTM_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                textBoxTM.Text = textBoxTM.Text.ToUpper().Trim();

                if (textBoxTM.Text == "")
                {
                    labelWARN.ForeColor = Color.Red;
                    labelWARN.Text = "��¼����Ʒ����";
                    return;
                }

                initTmVIEW();

                textBoxTM.SelectAll();
            }

        }

        private void initTmVIEW()
        {
            //�Ƿ�������¼
            sqlConn.Open();
            sqlComm.CommandText = "SELECT ��Ʒ�����.ID, ��Ʒ�����.���ݱ��, ��Ʒ�����.ժҪ, ��Ʒ�����.����, ��Ʒ�����.������� AS ����, ְԱ��.ְԱ���� AS ����Ա FROM ��Ʒ����� INNER JOIN ְԱ�� ON ��Ʒ�����.����ԱID = ְԱ��.ID WHERE (��Ʒ�����.���� = N'" + textBoxTM.Text.ToUpper() + "') ORDER BY ��Ʒ�����.����";
            if (dSet.Tables.Contains("�����")) dSet.Tables.Remove("�����");
            sqlDA.Fill(dSet, "�����");
            dataGridViewTM.DataSource = dSet.Tables["�����"];
            dataGridViewTM.Columns[0].Visible = false;

            if (dSet.Tables["�����"].Rows.Count < 1)
            {
                labelWARN.ForeColor = Color.Red;
                labelWARN.Text = "û����ؼ�¼";
            }
            else
            {
                labelWARN.ForeColor = Color.Green;
                labelWARN.Text = "��ȡ����ɹ�";
            }
            sqlConn.Close();
        }


        private void textBoxTM_Enter(object sender, EventArgs e)
        {
            textBoxTM.SelectAll();
        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "��Ʒ�����¼("+textBoxTM.Text.ToUpper()+");��";
            PrintDGV.Print_DataGridView(dataGridViewTM, strT, true, intUserLimit);
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "��Ʒ�����¼(" + textBoxTM.Text.ToUpper() + ");��";
            PrintDGV.Print_DataGridView(dataGridViewTM, strT, false, intUserLimit);
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            if (dataGridViewTM.SelectedRows.Count < 1)
            {
                MessageBox.Show("��ѡ��Ҫɾ�������룡", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (MessageBox.Show("�Ƿ�Ҫɾ��ѡ�������룿", "��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                return;

            System.Data.SqlClient.SqlTransaction sqlta;
            sqlConn.Open();
            sqlta = sqlConn.BeginTransaction();
            sqlComm.Transaction = sqlta;
            try
            {

                //��ϸ
                for (int i = 0; i < dataGridViewTM.SelectedRows.Count; i++)
                {

                    sqlComm.CommandText = "DELETE FROM ��Ʒ����� WHERE (ID = " + dataGridViewTM.SelectedRows[i].Cells[0].Value.ToString() + ")";
                    sqlComm.ExecuteNonQuery();

                }

                sqlta.Commit();
            }
            catch (Exception ex)
            {
                MessageBox.Show("���ݿ����" + ex.Message.ToString(), "���ݿ����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                sqlta.Rollback();
                return;
            }
            finally
            {
                sqlConn.Close();
            }
            MessageBox.Show("ɾ��������ϣ�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            initTmVIEW();
        }
    }
}