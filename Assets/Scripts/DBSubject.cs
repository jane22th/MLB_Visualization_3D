using UnityEngine;
using MySql.Data;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;


public class DBSubject : MonoBehaviour {
	MySqlConnection conn;
	MySqlDataReader rdr;

	public void connectDB() {
		string info = @"Server=163.239.201.196;Port=3306;Uid=root;Pwd=park;Database=gameday_mlb";

		conn = null;
		try {
			conn = new MySqlConnection(info);
			try {
				if (!conn.State.Equals(ConnectionState.Open)) {
					conn.Open();
				}
			} catch (MySqlException e) {
				Debug.Log(e);
			}
		} catch (MySqlException e) {
			Debug.Log(e);
		}
	}

	public void closeAllConn() {
		try {
			rdr.Dispose();
			conn.Close();
		} catch(Exception e) {
			Debug.Log(e);
		}
	}

	public List<object>[] requestQuery(string query) {
		connectDB();
		using (conn) {
			// command object
			MySqlCommand cmd = null;
			using (cmd = new MySqlCommand(query, conn)) {
				// reader object
				rdr = null;
				rdr = cmd.ExecuteReader();
				if(rdr.HasRows) {
					int list_len = 4;
					int col_cnt = rdr.FieldCount, idx=0;
					List<object>[] list = new List<object>[list_len];
					while(rdr.Read()) {
						list[idx] = new List<object>();
						for(int i=0; i<col_cnt; i++) {
							list[idx].Add(rdr.GetValue(i));
						}
						idx++;
						if(idx == list_len) {
							list_len <<= 1;
							Array.Resize<List<object>>(ref list, list_len);
						}
					}
					//Debug.Log ("list : " + list.Length);
					Array.Resize<List<object>>(ref list, idx);
					closeAllConn();
					return list;
				}
			}
		}
		closeAllConn();
		return null;
	}
}
