﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.XPath;
using System.Xml.Xsl;
using System.Xml;

namespace XML_QuanLyBanMayAnh
{
    internal class taoXML
    {
        string strCon = " Data Source =localhost; Initial Catalog = QuanLyBanMayAnh2; Integrated Security = True";
        public void TaoXML(string sql, string bang, string _FileXML)
        {
            SqlConnection con = new SqlConnection(strCon);
            con.Open();
            SqlDataAdapter ad = new SqlDataAdapter(sql, con);
            DataTable dt = new DataTable(bang);
            ad.Fill(dt);
            dt.WriteXml(_FileXML, XmlWriteMode.WriteSchema);
        }
        public DataTable loadDataGridView(string _FileXML)
        {
            DataTable dt = new DataTable();
            string FilePath = Application.StartupPath + _FileXML;
            if (File.Exists(FilePath))
            {
                //tao luong xu ly file xml
                FileStream fsReadXML = new FileStream(FilePath, FileMode.Open);
                //doc file xml vao datatable
                dt.ReadXml(fsReadXML);
                fsReadXML.Close();
            }
            else
            {
                MessageBox.Show("File không tồn tại");
            }
            return dt;
        }
        public void Them(string FileXML, string xml)
        {
            try
            {
                XmlTextReader textread = new XmlTextReader(FileXML);
                XmlDocument doc = new XmlDocument();
                doc.Load(textread);
                textread.Close();
                XmlNode currNode;
                XmlDocumentFragment docFrag = doc.CreateDocumentFragment();
                docFrag.InnerXml = xml;
                currNode = doc.DocumentElement;
                currNode.InsertAfter(docFrag, currNode.LastChild);
                doc.Save(FileXML);
            }
            catch
            {
                MessageBox.Show("lỗi");
            }
        }
        public void xoa(string _FileXML, string xml)
        {
            try
            {
                string fileName = Application.StartupPath + _FileXML;
                XmlDocument doc = new XmlDocument();
                doc.Load(fileName);
                XmlNode nodeCu = doc.SelectSingleNode(xml);
                doc.DocumentElement.RemoveChild(nodeCu);
                doc.Save(fileName);
            }
            catch
            {
                MessageBox.Show("lỗi");
            }
        }
        public void sua(string FileXML, string sql, string xml, string bang)
        {
            XmlTextReader reader = new XmlTextReader(FileXML);
            XmlDocument doc = new XmlDocument();
            doc.Load(reader);
            reader.Close();
            XmlNode oldValue;
            XmlElement root = doc.DocumentElement;
            oldValue = root.SelectSingleNode(sql);
            XmlElement newValue = doc.CreateElement(bang);
            newValue.InnerXml = xml;
            root.ReplaceChild(newValue, oldValue);
            doc.Save(FileXML);
        }
        public void TimKiem(string _FileXML, string xml, DataGridView dgv)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(Application.StartupPath + _FileXML);
            string xPath = xml;
            XmlNode node = xDoc.SelectSingleNode(xPath);
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            XmlNodeReader nr = new XmlNodeReader(node);
            ds.ReadXml(nr);
            dgv.DataSource = ds.Tables[0];
            nr.Close();
        }
        public string LayGiaTri(string duongDan, string truongA, string giaTriA, string truongB)
        {
            string giatriB = "";
            DataTable dt = new DataTable();
            dt = loadDataGridView(duongDan);
            int soDongNhanVien = dt.Rows.Count;
            for (int i = 0; i < soDongNhanVien; i++)
            {
                if (dt.Rows[i][truongA].ToString().Trim().Equals(giaTriA))
                {
                    giatriB = dt.Rows[i][truongB].ToString();
                    return giatriB;
                }
            }
            return giatriB;
        }
        public bool KiemTra(string _FileXML, string truongKiemTra, string giaTriKiemTra)
        {
            DataTable dt = new DataTable();
            dt = loadDataGridView(_FileXML);
            dt.DefaultView.RowFilter = truongKiemTra + " ='" + giaTriKiemTra + "'";
            if (dt.DefaultView.Count > 0)
                return true;
            return false;
        }
        public string txtMa(string tienTo, string _FileXML, string tenCot)
        {
            string txtMa = "";
            DataTable dt = new DataTable();
            dt = loadDataGridView(_FileXML);
            int dem = dt.Rows.Count;
            if (dem == 0)
            {
                txtMa = tienTo + "001";//HD001
            }
            else
            {
                int duoi = int.Parse(dt.Rows[dem - 1][tenCot].ToString().Substring(2, 3)) + 1;
                string cuoi = "00" + duoi;
                txtMa = tienTo + "" + cuoi.Substring(cuoi.Length - 3, 3);
            }
            return txtMa;
        }
        public bool KTMa(string _FileXML, string cotMa, string ma)
        {
            bool kt = true;
            DataTable dt = new DataTable();
            dt = loadDataGridView(_FileXML);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i][cotMa].ToString().Trim().Equals(ma))
                {
                    kt = false;
                }
                else
                {
                    kt = true;
                }
            }
            return kt;
        }
        public void exCuteNonQuery(string sql)
        {
            SqlConnection con = new SqlConnection(strCon);
            con.Open();
            SqlCommand com = new SqlCommand(sql, con);
            com.ExecuteNonQuery();
        }
        public void Them_Database(string tenBang, string _FileXML)
        {
            string duongDan = _FileXML;
            DataTable table = loadDataGridView(duongDan);
            int dong = table.Rows.Count - 1;
            string sql = "insert into " + tenBang + " values(";
            for (int j = 0; j < table.Columns.Count - 1; j++)
            {
                sql += "N'" + table.Rows[dong][j].ToString().Trim() + "',";
            }
            sql += "N'" + table.Rows[dong][table.Columns.Count - 1].ToString().Trim() + "'";
            sql += ")";
            exCuteNonQuery(sql);
        }
        public void Sua_Database(string tenBang, string _FileXML, string tenCot, string giaTri)
        {
            string duongDan = _FileXML;
            DataTable table = loadDataGridView(duongDan);
            int dong = -1;
            for (int i = 0; i < table.Rows.Count; i++)
            {
                if (table.Rows[i][tenCot].ToString().Trim() == giaTri)
                { dong = i; }
            }
            if (dong > -1)
            {
                string sql = "update " + tenBang + " set ";
                for (int j = 0; j < table.Columns.Count - 1; j++)
                {
                    sql += table.Columns[j].ToString() + " = N'" + table.Rows[dong][j].ToString().Trim() + "', ";
                }
                sql += table.Columns[table.Columns.Count - 1].ToString() + " = N'" + table.Rows[dong][table.Columns.Count - 1].ToString().Trim() + "' ";
                sql += "where " + tenCot + "= '" + giaTri + "'";
                exCuteNonQuery(sql);
            }
        }
        public void Xoa_Database(string _FileXML, string tenCot, string giaTri, string tenBang)
        {
            string duongDan = _FileXML;
            DataTable table = loadDataGridView(duongDan);
            int dong = -1;
            for (int i = 0; i < table.Rows.Count; i++)
            {
                if (table.Rows[i][tenCot].ToString().Trim() == giaTri)
                { dong = i; }
            }
            if (dong > -1)
            {
                string sql = "delete from " + tenBang + " where ";
                for (int j = 0; j < table.Columns.Count - 1; j++)
                {
                    if (table.Rows[dong][tenCot].ToString().Trim() == giaTri)
                    {
                        sql += tenCot + " = '" + giaTri + "'";
                    }
                }
                exCuteNonQuery(sql);
            }
        }
        public void CapNhapTungBang(string tenBang, string _FileXML)
        {
            string duongDan = _FileXML;
            DataTable table = loadDataGridView(duongDan);
            for (int i = 0; i < table.Rows.Count; i++)
            {
                string sql = "insert into " + tenBang + " values(";
                for (int j = 0; j < table.Columns.Count - 1; j++)
                {
                    sql += "N'" + table.Rows[i][j].ToString().Trim() + "',";
                }
                sql += "N'" + table.Rows[i][table.Columns.Count - 1].ToString().Trim() + "'";
                sql += ")";
                exCuteNonQuery(sql);
            }

        }
        public void TimKiemXSLT(string data, string tenFileXML, string tenfileXSLT)
        {
            XslCompiledTransform xslt = new XslCompiledTransform();
            xslt.Load("" + tenfileXSLT + ".xslt");
            XsltArgumentList argList = new XsltArgumentList();
            argList.AddParam("Data", "", data);
            XmlWriter writer = XmlWriter.Create("" + tenFileXML + ".html");
            xslt.Transform(new XPathDocument("" + tenFileXML + ".xml"), argList, writer);
            writer.Close();
            System.Diagnostics.Process.Start("" + tenFileXML + ".html");
        }
    }

}