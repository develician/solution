using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;
using System.Runtime.ExceptionServices;
using System.Security;
using System.Threading;

namespace ats
{
    public partial class Form1 : Form
    {
        int g_scr_no = 0;

        string g_user_id = null;
        string g_accnt_no = null;

        int g_is_thread = 0;
        Thread thread1 = null;

        int g_flag_1 = 0;

        string g_rqname = null;

        int g_ord_amt_possible = 0;

        int g_flag_2 = 0;
        int g_is_next = 0;

        int g_flag_3 = 0; // 매수 주문 응답 플래그
        int g_flag_4 = 0; // 매도주문 응답 플래그
        int g_flag_5 = 0; // 매도취소주문 응답 플래그

        int g_buy_hoga = 0; // 최우선 매수호가 저장 변수
        int g_flag_7 = 0; // 최우선 매수호가 클래스 벼누가 1이면 조회 완료

        int g_cur_price = 0; // 현재가
        int g_flag_6 = 0; // 현재가 조회 플래그 변수가 1이면 조회 완료

        string OracleUserID = "ats";
        string OracleUserPassword = "root1234";
        string OracleHost = "localhost";
        string OraclePort = "1521";
        string OracleSid = "xe";

        public Form1()
        {
            InitializeComponent();
            this.jd_kw_openAPI.OnReceiveTrData += new AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveTrDataEventHandler(this.axKHOpenAPI1_OnReceiveTrData);
            this.jd_kw_openAPI.OnReceiveMsg += new AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveMsgEventHandler(this.axKHOpenAPI1_OnReceiveMsg);
            this.jd_kw_openAPI.OnReceiveChejanData += new AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveChejanDataEventHandler(this.axKHOpenAPI1_OnReceiveChejanData);
            this.jd_dgv_main.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            this.jd_dgv_main.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            this.jd_dgv_main.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
            this.jd_dgv_main.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            this.jd_dgv_main.Columns[4].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            this.jd_dgv_main.Columns[5].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            this.jd_dgv_main.Columns[6].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            this.jd_dgv_main.Columns[7].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            this.jd_dgv_main.Columns[8].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.jd_dgv_main.Columns[9].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.jd_dgv_main.Columns[10].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        public string get_cur_tm()
        {
            DateTime l_cur_time;
            string l_cur_tm;

            l_cur_time = DateTime.Now;
            l_cur_tm = l_cur_time.ToString("HHmmss");
            return l_cur_tm;
        }

        public string get_jongmok_nm(string i_jongmok_cd)
        {
            string l_jongmok_nm = null;

            l_jongmok_nm = jd_kw_openAPI.GetMasterCodeName(i_jongmok_cd);
            return l_jongmok_nm;
        }

        private OracleConnection connect_db()
        {
            String conninfo = "User Id=" + OracleUserID + ";" +
                "Password = " + OracleUserPassword + ";" +
                "Data Source = (DESCRIPTION=" +
                "(ADDRESS=(PROTOCOL=TCP)(HOST=" + OracleHost + ")(PORT=" + OraclePort + "))" +
                "(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=" + OracleSid +  ")) );";
            

            OracleConnection conn = new OracleConnection(conninfo);

            try
            {
                conn.Open();
            } catch(Exception ex)
            {
                MessageBox.Show("connect_db() FAIL! " + ex.Message, "오류 발생");
                conn = null;
            }

            return conn;
        }

        public void write_msg_log(String text, int is_clear)
        {
            DateTime l_cur_time;
            String l_cur_dt;

            String l_cur_tm;
            String l_cur_dtm;

            l_cur_dt = "";
            l_cur_tm = "";

            l_cur_time = DateTime.Now;
            l_cur_dt = l_cur_time.ToString("yyyy-") + l_cur_time.ToString("MM-") + l_cur_time.ToString("dd");
            l_cur_tm = l_cur_time.ToString("HH:mm:ss");

            l_cur_dtm = "[" + l_cur_dt + " " + l_cur_tm + "]";

            if (is_clear == 1)
            {
                if(this.jd_txt_msg.InvokeRequired)
                {
                    jd_txt_user_id.BeginInvoke(new Action(() => jd_txt_msg.Clear()));
                } else
                {
                    this.jd_txt_msg.Clear();
                }
            } else
            {
                if(this.jd_txt_msg.InvokeRequired)
                {
                    jd_txt_msg.BeginInvoke(new Action(() => jd_txt_msg.AppendText(l_cur_dtm + text)));
                } else
                {
                    this.jd_txt_msg.AppendText(l_cur_dtm + text);
                }
            }
        }

        public void write_err_log(String text, int is_clear)
        {
            DateTime l_cur_time;
            String l_cur_dt;

            String l_cur_tm;
            String l_cur_dtm;

            l_cur_dt = "";
            l_cur_tm = "";

            l_cur_time = DateTime.Now;
            l_cur_dt = l_cur_time.ToString("yyyy-") + l_cur_time.ToString("MM-") + l_cur_time.ToString("dd");
            l_cur_tm = l_cur_time.ToString("HH:mm:ss");

            l_cur_dtm = "[" + l_cur_dt + " " + l_cur_tm + "]";

            if (is_clear == 1)
            {
                if (this.jd_txt_err.InvokeRequired)
                {
                    jd_txt_user_id.BeginInvoke(new Action(() => jd_txt_err.Clear()));
                }
                else
                {
                    this.jd_txt_err.Clear();
                }
            }
            else
            {
                if (this.jd_txt_err.InvokeRequired)
                {
                    jd_txt_msg.BeginInvoke(new Action(() => jd_txt_err.AppendText(l_cur_dtm + text)));
                }
                else
                {
                    this.jd_txt_err.AppendText(l_cur_dtm + text);
                }
            }
        }

        [HandleProcessCorruptedStateExceptions]
        [SecurityCritical]
        public DateTime delay(int MS)
        {
            DateTime ThisMoment = DateTime.Now;
            TimeSpan duration = new TimeSpan(0, 0, 0, 0, MS);
            DateTime AfterWards = ThisMoment.Add(duration);

            while(AfterWards >= ThisMoment)
            {
                try
                {
                    unsafe
                    {
                        System.Windows.Forms.Application.DoEvents();

                    }
                } catch(AccessViolationException ex)
                {
                    write_err_log("delay() ex.Message : [" + ex.Message + "]\n", 0);
                }
                ThisMoment = DateTime.Now;
            }

            return DateTime.Now;
        }

        private string get_scr_no()
        {
            if(g_scr_no < 9999)
            {
                g_scr_no++;
            } else
            {
                g_scr_no = 1000;
            }
            return g_scr_no.ToString();
        }

        public void m_thread1()
        {
            string l_cur_tm = null;
            int l_set_tb_accnt_flag = 0; // 1이면 호출 완료
            int l_set_tb_accnt_info_flag = 0; // 1이면 호출 완료
            int l_sell_ord_first_flag = 0; // 1이면 호출 완료

            if (g_is_thread == 0)
            {
                g_is_thread = 1; // 중복 스레드 생성 방지
                write_msg_log("자동 매매가 시작되었습니다.\n", 0);
            }
            for (; ; )
            {
                l_cur_tm = get_cur_tm();
                if (l_cur_tm.CompareTo("083001") >= 0) // After 8:30 am
                {
                    // TODO: 계좌 조회, 계좌 정보 조회, 보유종목 매도주문 수행
                    if(l_set_tb_accnt_flag == 0) // 호출 전
                    {
                        l_set_tb_accnt_flag = 1; // 호출로 설정
                        set_tb_accnt(); // 호출 하기
                    }
                    if (l_set_tb_accnt_info_flag == 0)
                    {
                        set_tb_accnt_info();
                        l_set_tb_accnt_info_flag = 1;
                    }
                    if(l_sell_ord_first_flag == 0)
                    {
                        sell_ord_first(); // 보유 종목 매도
                        l_sell_ord_first_flag = 1;

                    }
                }
                if (l_cur_tm.CompareTo("090001") >= 0) // After 09:00 am
                {
                    for (; ; )
                    {
                        l_cur_tm = get_cur_tm();
                        if (l_cur_tm.CompareTo("153001") >= 0) // After 15:30 pm
                        {
                            break;
                        }
                        // TODO: 장 운영시간 중이므로 매수나 매도 주문
                        real_buy_ord();

                        delay(200); // 0.2초 지연

                        real_sell_ord(); // 실시간 매도주문 메서드 호출

                        delay(200);
                        real_cut_loss_ord(); // 실시간 손절 주문 메서드 호출
                    }
                }
                delay(200); // 첫번째 무한루프 지연
            }

            
        }

        public void set_tb_accnt()
        {
            int l_for_cnt = 0;
            int l_for_flag = 0;

            write_msg_log("TB_ACCNT 테이블 세팅 시작\n", 0);

            g_ord_amt_possible = 0;

            l_for_flag = 0;
            for(; ; )
            {
                jd_kw_openAPI.SetInputValue("계좌번호", g_accnt_no);
                // 에러 구간
                jd_kw_openAPI.SetInputValue("비밀번호", "");

                g_rqname = "";
                g_rqname = "증거금세부내역조회요청"; // 요청 명 정의
                g_flag_1 = 0; // 요청 중

                String l_scr_no = null;
                l_scr_no = "";
                l_scr_no = get_scr_no();
                jd_kw_openAPI.CommRqData("증거금세부내역조회요청", "opw00013", 0, l_scr_no);
                // OpenAPI로 데이터 request

                l_for_cnt = 0;
                for(; ; )
                {
                    if(g_flag_1 == 1)
                    {
                        delay(1000);
                        jd_kw_openAPI.DisconnectRealData(l_scr_no);
                        l_for_flag = 1;
                        break;
                    } else
                    {
                        write_msg_log("'증거금세부내역조회요청' 완료 대기중...\n", 0);
                        delay(1000);
                        l_for_cnt++;
                        if (l_for_cnt == 1) // 한번이라도 실패시 무한루프 break (증권계좌 비밀번호 오류방지)
                        {
                            l_for_flag = 0;
                            break;
                        } else
                        {
                            continue;
                        }
                    }
                }

                jd_kw_openAPI.DisconnectRealData(l_scr_no);

                if(l_for_flag == 1) // 요청에대한 응답 받았으므로 무한루프 탈출
                {
                    break;
                }
                else if (l_for_flag == 0) // 요청에 대한 응답 받지 못해도 비밀번호 5회 오류 방지 위해 무한루프 탈출
                {
                    delay(1000);
                    break; // 비번 5회 오류 방지
                }
                delay(1000);
            }
            write_msg_log("매수가능금액 : [" + g_ord_amt_possible.ToString() + "]\n", 0);
            merge_tb_accnt(g_ord_amt_possible);
        }

        public void merge_tb_accnt(int g_ord_amt_possible)
        {
            OracleCommand cmd = null;
            OracleConnection conn = null;
            String l_sql = null;

            l_sql = null;
            cmd = null;
            conn = null;
            conn = connect_db();

            if(conn != null)
            {
                cmd = new OracleCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;

                //l_sql = @"merge into tb_accnt a using (" +
                //    "select nvl(max(user_id), ' ') user_id, nvl(max(accnt_no), ' ') accnt_no, nvl(max(ref_dt), ' ') ref_dt " +
                //    " from tb_accnt " +
                //    " where user_id = '" + g_user_id + "'" +
                //    "and accnt_no = " + "'" + g_accnt_no + "'" +
                //    "and ref_dt = to_char(sysdate, 'yyyymmdd') " +
                //    " ) b " +
                //    " on ( a.user_id = b.user_id and a.accnt_no = b.accnt_no and a.ref_dt = b.ref_dt) " +
                //    " when matched then update " +
                //    " set ord_possible_amt = " + g_ord_amt_possible + "," +
                //    " updt_dtm = SYSDATE" + "," +
                //    " updt_id = 'ats'" +
                //    " when not matched then insert (a.user_id, a.accnt_no, a.ref_dt, a.ord_possible_amt, a.inst_dtm, a.inst_id) values ( " +
                //    "'" + g_user_id + "'" + "," +
                //    "'" + g_accnt_no + "'" + "," +
                //    " to_char(sysdate, 'yyyymmdd') " + "," +
                //    g_ord_amt_possible + "," +
                //    "SYSDATE, " +
                //    "'ats'" +
                //    " )";

                l_sql = "merge into tb_accnt a using (" +
                    "select nvl(max(user_id), ' ') user_id, nvl(max(accnt_no), ' ') accnt_no, nvl(max(ref_dt), ' ') ref_dt " +
                    " from tb_accnt " +
                    " where user_id = :g_user_id" +
                    "and accnt_no = :g_accnt_no" +
                    "and ref_dt = to_char(sysdate, 'yyyymmdd') " +
                    " ) b " +
                    " on ( a.user_id = b.user_id and a.accnt_no = b.accnt_no and a.ref_dt = b.ref_dt) " +
                    " when matched then update " +
                    " set ord_possible_amt = :g_ord_amt_possible," +
                    " updt_dtm = SYSDATE" + "," +
                    " updt_id = 'ats'" +
                    " when not matched then insert (a.user_id, a.accnt_no, a.ref_dt, a.ord_possible_amt, a.inst_dtm, a.inst_id) values ( " +
                    ":g_user_id, :g_accnt_no," +
                    " to_char(sysdate, 'yyyymmdd') , :g_ord_amt_possible," +
                    "SYSDATE, " +
                    "'ats'" +
                    " )";
                cmd.CommandText = l_sql;
                cmd.Parameters.Add("g_user_id", g_user_id);
                cmd.Parameters.Add("g_accnt_no", g_accnt_no);
                cmd.Parameters.Add("g_ord_amt_possible", g_ord_amt_possible);







                try
                {
                    cmd.ExecuteNonQuery();
                } catch(Exception ex)
                {
                    write_err_log("merge_tb_accnt() ex : [" + ex.Message + "]\n", 0);
                } finally
                {
                    conn.Close();
                }
            } else
            {
                write_msg_log("db connection check!\n", 0);
            }
        }

        public void merge_tb_accnt_info(String i_jongmok_cd, String i_jongmok_nm, int i_boyu_cnt, int i_boyu_price, int i_boyu_amt)
            // 계좌정보 테이블 세팅 메서드
        {
            OracleCommand cmd = null;
            OracleConnection conn = null;
            String l_sql = null;

            l_sql = null;
            cmd = null;
            conn = null;
            conn = connect_db();

            cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;

            //l_sql = @"merge into tb_accnt_info a using (" +
            //        "select nvl(max(user_id), '0') user_id, nvl(max(ref_dt), '0') ref_dt, nvl(max(jongmok_cd), '0') jongmok_cd, nvl(max(jongmok_nm), '0') jongmok_nm" +
            //        " from tb_accnt_info " +
            //        " where user_id = '" + g_user_id + "'" +
            //        "and accnt_no = " + "'" + g_accnt_no + "'" +
            //        "and jongmok_cd = " + "'" + i_jongmok_cd + "'" +
            //        "and ref_dt = to_char(sysdate, 'yyyymmdd') " +
            //        " ) b " +
            //        " on ( a.user_id = b.user_id and a.jongmok_cd = b.jongmok_cd and a.ref_dt = b.ref_dt) " +
            //        " when matched then update " +
            //        " set OWN_STOCK_CNT = " + i_boyu_cnt + "," +
            //        " BUY_PRICE = " + i_boyu_price + "," +
            //        " OWN_AMT = " + i_boyu_amt + "," +
            //        " updt_dtm = SYSDATE" + "," + 
            //        " updt_id = 'ats'" +
            //        " when not matched then insert (a.user_id, a.accnt_no, a.ref_dt, a.jongmok_cd, a.jongmok_nm, a.BUY_PRICE, a.OWN_STOCK_CNT, a.OWN_AMT, a.inst_dtm, a.inst_id) values ( " +
            //        "'" + g_user_id + "'" + "," +
            //        "'" + g_accnt_no + "'" + "," +
            //        " to_char(sysdate, 'yyyymmdd') " + "," +
            //        "'" + i_jongmok_cd + "'" + "," +
            //        "'" + i_jongmok_nm + "'" + "," +
            //        i_boyu_price + "," +
            //        i_boyu_cnt + "," +
            //        i_boyu_amt + "," +
            //        "SYSDATE, " +
            //        "'ats'" +
            //        " )";

            l_sql = @"merge into tb_accnt_info a using (" +
                    "select nvl(max(user_id), '0') user_id, nvl(max(ref_dt), '0') ref_dt, nvl(max(jongmok_cd), '0') jongmok_cd, nvl(max(jongmok_nm), '0') jongmok_nm" +
                    " from tb_accnt_info " +
                    " where user_id = :g_user_id " +
                    "and accnt_no = :g_accnt_no " +
                    "and jongmok_cd = :i_jongmok_cd " +
                    "and ref_dt = to_char(sysdate, 'yyyymmdd') " +
                    " ) b " +
                    " on ( a.user_id = b.user_id and a.jongmok_cd = b.jongmok_cd and a.ref_dt = b.ref_dt) " +
                    " when matched then update " +
                    " set OWN_STOCK_CNT = :i_boyu_cnt," +
                    " BUY_PRICE = :i_boyu_price," +
                    " OWN_AMT = :i_boyu_amt," + 
                    " updt_dtm = SYSDATE" + "," +
                    " updt_id = 'ats'" +
                    " when not matched then insert (a.user_id, a.accnt_no, a.ref_dt, a.jongmok_cd, a.jongmok_nm, a.BUY_PRICE, a.OWN_STOCK_CNT, a.OWN_AMT, a.inst_dtm, a.inst_id) values ( " +
                    ":g_user_id, :g_accnt_no, " +
                    " to_char(sysdate, 'yyyymmdd') " + "," +
                    ":i_jongmok_cd, :i_jongmok_nm, :i_boyu_price, :i_boyu_cnt, i_boyu_amt," +
                    "SYSDATE, " +
                    "'ats'" +
                    " )";

            cmd.CommandText = l_sql;
            cmd.Parameters.Add("g_user_id", g_user_id);
            cmd.Parameters.Add("g_accnt_no", g_accnt_no);
            cmd.Parameters.Add("i_jongmok_cd", i_jongmok_cd);
            cmd.Parameters.Add("i_boyu_cnt", i_boyu_cnt);
            cmd.Parameters.Add("i_boyu_price", i_boyu_price);
            cmd.Parameters.Add("i_boyu_amt", i_boyu_amt);
            cmd.Parameters.Add("i_jongmok_nm", i_jongmok_nm);

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                write_err_log("merge TB_ACCNT_INFO ex : [" + ex.Message + "]\n", 0);
            }

            conn.Close();
        }

        public void set_tb_accnt_info()
        {
            OracleCommand cmd;
            OracleConnection conn;
            String sql;
            int l_for_cnt = 0;
            int l_for_flag = 0;

            sql = null;
            cmd = null;

            conn = null;
            conn = connect_db();

            cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;

            //sql = @"delete from tb_accnt_info where ref_dt = to_char(sysdate, 'yyyymmdd') and user_id = " + "'" + g_user_id + "'";
            sql = @"delete from tb_accnt_info where ref_dt = to_char(sysdate, 'yyyymmdd') and user_id = :g_user_id";

            cmd.CommandText = sql;
            cmd.Parameters.Add("g_user_id", g_user_id);

            try
            {
                cmd.ExecuteNonQuery();
            } catch(Exception ex)
            {
                write_err_log("delete tb_accnt_info ex.Message : [" + ex.Message + "]\n", 0);
            }

            conn.Close();

            g_is_next = 0;

            for(; ; )
            {
                l_for_flag = 0;
                for(; ; )
                {
                    jd_kw_openAPI.SetInputValue("계좌번호", g_accnt_no);
                    jd_kw_openAPI.SetInputValue("비밀번호", "");
                    jd_kw_openAPI.SetInputValue("상장폐지조회구분", "1");
                    jd_kw_openAPI.SetInputValue("비밀번호입력매체구분", "00");

                    g_flag_2 = 0;
                    g_rqname = "계좌평가현황요청";

                    String l_scr_no = get_scr_no();

                    jd_kw_openAPI.CommRqData("계좌평가현황요청", "OPW00004", g_is_next, l_scr_no);

                    l_for_cnt = 0;
                    for (; ; )
                    {
                        if (g_flag_2 == 1)
                        {
                            delay(1000);
                            jd_kw_openAPI.DisconnectRealData(l_scr_no);
                            l_for_flag = 1;

                            break;
                        } else
                        {
                            delay(1000);
                            l_for_flag++;
                            if (l_for_cnt == 5)
                            {
                                l_for_flag = 0;
                                break;
                            } else
                            {
                                continue;
                            }
                        }

                        delay(1000);
                        jd_kw_openAPI.DisconnectRealData(l_scr_no);
                        if (l_for_flag == 1)
                        {
                            break;
                        }
                        else if (l_for_flag == 0)
                        {
                            delay(1000);
                            continue;
                        }
                    }

                    if(g_is_next == 0)
                    {
                        break;
                    }
                    delay(1000);

                }
            }
        }

        public void insert_tb_accnt_info(string i_jongmok_cd, string i_jongmok_nm, int i_buy_price, int i_own_stock_cnt,
            int i_own_amt)
        {
            OracleCommand cmd = null;
            OracleConnection conn = null;
            String l_sql = null;

            l_sql = null;
            cmd = null;
            conn = null;
            conn = connect_db();

            cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;

            //l_sql = @"insert into tb_accnt_info values (" +
            //    "'" + g_user_id + "'" + "," +
            //    "'" + g_accnt_no + "'" + "," +
            //    "to_char(sysdate, 'yyyymmdd')" + "," +
            //    "'" + i_jongmok_cd + "'" + "," +
            //    "'" + i_jongmok_nm + "'" + "," +
            //    i_buy_price + "," +
            //    i_own_stock_cnt + "," +
            //    i_own_amt + "," +
            //    "'ats'" + "," +
            //    "SYSDATE" + "," + "null" + "," + "null" + ") ";

            l_sql = @"insert into tb_accnt_info values (" +
                ":g_user_id, " +
                ":g_accnt_no, " +
                "to_char(sysdate, 'yyyymmdd')" + "," +
                ":i_jongmok_cd, :i_jongmok_nm, :i_buy_price, :i_own_stock_cnt, :i_own_amt," +
                "'ats'" + "," +
                "SYSDATE" + "," + "null" + "," + "null" + ") ";

            cmd.CommandText = l_sql;
            cmd.Parameters.Add("i_jongmok_cd", i_jongmok_cd);
            cmd.Parameters.Add("i_jongmok_nm", i_jongmok_nm);
            cmd.Parameters.Add("i_buy_price", i_buy_price);
            cmd.Parameters.Add("i_own_stock_cnt", i_own_stock_cnt);
            cmd.Parameters.Add("i_own_amt", i_own_amt);

            try
            {
                cmd.ExecuteNonQuery();
            } catch(Exception ex)
            {
                write_err_log("insert tb_accnt_info() ex.Message: [" + ex.Message + "]\n", 0);
            }
            conn.Close();
        }

        public void insert_tb_ord_lst(string i_ref_dt, String i_jongmok_cd, String i_jongmok_nm, String i_ord_gb, String i_ord_no, String i_org_ord_no, int i_ord_price
            , int i_ord_stock_cnt, int i_ord_amt, String i_ord_dtm)
        {
            OracleCommand cmd = null;
            OracleConnection conn = null;
            String l_sql = null;

            l_sql = null;
            cmd = null;
            conn = null;
            conn = connect_db();

            cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;

            //l_sql = @"INSERT INTO tb_ord_lst VALUES ( " +
            //    "'" + g_user_id + "'" + "," +
            //    "'" + g_accnt_no + "'" + "," +
            //    "'" + i_ref_dt + "'" + "," +
            //    "'" + i_jongmok_cd + "'" + "," +
            //    "'" + i_ord_gb + "'" + "," +
            //    "'" + i_ord_no + "'" + "," +
            //    "'" + i_org_ord_no + "'" + "," +
            //    i_ord_price + "," +
            //    i_ord_stock_cnt + "," +
            //    i_ord_amt + "," +
            //    "'" + i_ord_dtm + "'" + "," +
            //    "'ats'" + "," +
            //    "SYSDATE" + "," +
            //    "null" + "," +
            //    "null" + ")";

            l_sql = @"INSERT INTO tb_ord_lst VALUES ( " +
                ":g_user_id, :g_accnt_no, :i_ref_dt, :i_jongmok_cd, :i_ord_gb, :i_ord_no, :i_org_ord_no, " +
                ":i_ord_price, :i_ord_stock_cnt, :i_ord_amt, :i_ord_dtm, " +
                "'ats'" + "," +
                "SYSDATE" + "," +
                "null" + "," +
                "null" + ")";


            cmd.CommandText = l_sql;
            cmd.Parameters.Add("g_user_id", g_user_id);
            cmd.Parameters.Add("g_accnt_no", g_accnt_no);
            cmd.Parameters.Add("i_ref_dt", i_ref_dt);
            cmd.Parameters.Add("i_jongmok_cd", i_jongmok_cd);
            cmd.Parameters.Add("i_ord_gb", i_ord_gb);
            cmd.Parameters.Add("i_ord_no", i_ord_no);
            cmd.Parameters.Add("i_org_ord_no", i_org_ord_no);
            cmd.Parameters.Add("i_ord_price", i_ord_price);
            cmd.Parameters.Add("i_ord_stock_cnt", i_ord_stock_cnt);
            cmd.Parameters.Add("i_ord_amt", i_ord_amt);
            cmd.Parameters.Add("i_ord_dtm", i_ord_dtm);

            try
            {
                cmd.ExecuteNonQuery();
            } catch(Exception ex)
            {
                write_err_log("insert tb_ord_lst ex : [" + ex.Message + "] \n", 0);
            }

            conn.Close();
        }

        public void update_tb_accnt(String i_chegyul_gb, int i_chegyul_amt)
        {
            OracleCommand cmd = null;
            OracleConnection conn = null;
            String l_sql = null;

            l_sql = null;

            cmd = null;
            conn = null;
            conn = connect_db();

            cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;

            if(i_chegyul_gb == "2")
            {
                l_sql = @"UPDATE TB_ACCNT set ORD_POSSIBLE_AMT = ord_possible_amt - " +
                    ":i_chegyul_amt, updt_dtm = SYSDATE, updt_id = 'ats' " +
                    " WHERE user_id = :g_user_id" +
                    " and accnt_no = :g_accnt_no" +
                    " and ref_dt = to_char(sysdate, 'yyyymmdd') ";
                

            }
            else if(i_chegyul_gb == "1")
            {
                l_sql = @"UPDATE TB_ACCNT SET ORD_POSSIBLE_AMT = ord_possible_amt + " +
                    ":i_chegyul_amt, updt_dtm = SYSDATE, updt_id = 'ats' " +
                    " WHERE user_id = :g_user_id" +
                    " AND accnt_no = :g_accnt_no" +
                    " AND ref_dt = to_char(sysdate, 'yyyymmdd') ";
                
            }

            cmd.CommandText = l_sql;
            cmd.Parameters.Add("i_chegyul_amt", i_chegyul_amt);
            cmd.Parameters.Add("g_user_id", g_user_id);
            cmd.Parameters.Add("g_accnt_no", g_accnt_no);


            try
            {
                cmd.ExecuteNonQuery();
            } catch(Exception ex)
            {
                write_err_log("update TB_ACCNT ex.Message : [" + ex.Message + "] \n", 0);
            }
            conn.Close();
        }

        public void real_buy_ord() {
            OracleCommand cmd = null;
            OracleConnection conn = null;
            String sql = null;
            OracleDataReader reader = null;

            string l_jongmok_cd = null;
            int l_buy_amt = 0;
            int l_buy_price = 0;

            conn = null;
            conn = connect_db();

            sql = null;
            cmd = null;
            reader = null;

            cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;

            //sql = @" SELECT A.JONGMOK_CD, A.BUY_AMT, A.BUY_PRICE FROM TB_TRD_JONGMOK A WHERE A.USER_ID = '" + g_user_id + "'" + 
            //" AND A.BUY_TRD_YN = 'Y' ORDER BY A.PRIORITY";

            sql = @"SELECT A.JONGMOK_CD, A.BUY_AMT, A.BUY_PRICE FROM TB_TRD_JONGMOK A WHERE A.USER_ID = :g_user_id" +
            " AND A.BUY_TRD_YN = 'Y' ORDER BY A.PRIORITY";

            cmd.CommandText = sql;
            cmd.Parameters.Add("g_user_id", g_user_id);

            reader = cmd.ExecuteReader();

            while (reader.Read()) {
                l_jongmok_cd = "";
                l_buy_amt = 0;
                l_buy_price = 0;

                l_jongmok_cd = reader[0].ToString().Trim();
                l_buy_amt = int.Parse(reader[1].ToString().Trim());
                l_buy_price = int.Parse(reader[2].ToString().Trim());

                int l_buy_price_tmp = 0;
                l_buy_price_tmp = get_hoga_unit_price(l_buy_price, l_jongmok_cd, 1);

                int l_buy_ord_stock_cnt = 0;
                l_buy_ord_stock_cnt = (int)(l_buy_amt / l_buy_price_tmp);
                write_msg_log("종목코드 : [" + l_jongmok_cd.ToString() + " ]\n", 0);
                write_msg_log("종목명 : [" + get_jongmok_nm(l_jongmok_cd) + " ]\n", 0);
                write_msg_log("매수금액 : [" + l_buy_amt.ToString() + " ]\n", 0);
                write_msg_log("매수가 : [" + l_buy_price_tmp.ToString() + " ]\n", 0);

                int l_own_stock_cnt = 0;
                l_own_stock_cnt = get_own_stock_cnt(l_jongmok_cd);
                write_msg_log("보유주식수 : [" + l_own_stock_cnt.ToString() + "]\n", 0);

                if (l_own_stock_cnt > 0) {
                    write_msg_log("해당 종목을 보유 중이므로 매수하지 않음\n", 0);
                    continue;
                }

                string l_buy_not_chegyul_yn = null;
                l_buy_not_chegyul_yn = get_buy_not_chegyul_yn(l_jongmok_cd);

                int l_for_flag = 0;
                int l_for_cnt = 0;
                l_for_flag = 0;
                g_buy_hoga = 0;

                for (; ; )
                {
                    g_rqname = "";
                    g_rqname = "호가조회";
                    g_flag_7 = 0;
                    jd_kw_openAPI.SetInputValue("종목코드", l_jongmok_cd);

                    string l_scr_no_2 = null;
                    l_scr_no_2 = "";
                    l_scr_no_2 = get_scr_no();

                    jd_kw_openAPI.CommRqData("호가조회", "opt10004", 0, l_scr_no_2);

                    try
                    {
                        l_for_cnt = 0;
                        for (; ; )
                        {
                            if (g_flag_7 == 1)
                            {
                                delay(200);
                                jd_kw_openAPI.DisconnectRealData(l_scr_no_2);
                                l_for_flag = 1;
                                break;
                            }
                            else
                            {
                                write_msg_log("'호가조회' 완료 대기중 ... \n", 0);
                                delay(200);
                                l_for_cnt++;
                                if (l_for_cnt == 5)
                                {
                                    l_for_flag = 0;
                                    break;
                                }
                                else
                                {
                                    continue;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        write_err_log("real_buy_ord() 호가조회 ex.Message : [" + ex.Message + "]\n", 0);
                    }

                    jd_kw_openAPI.DisconnectRealData(l_scr_no_2);

                    if(l_for_flag == 1)
                    {
                        break;
                    } else if(l_for_flag == 0)
                    {
                        delay(200);
                        continue;
                    }
                    delay(200);
                }

                if (l_buy_not_chegyul_yn == "Y")
                {
                    write_msg_log("해당 종목에 미체결 매수주문이 있으므로 매수하지 않음. \n", 0);
                    continue;
                }

                if(l_buy_price > g_buy_hoga)
                {
                    write_msg_log("해당 종목의 매수가가 최우선 매수호가보다 크므로 매수주문하지 않음 \n", 0);
                    continue;
                }

                g_flag_3 = 0;
                g_rqname = "매수주문";

                String l_scr_no = null;
                l_scr_no = "";
                l_scr_no = get_scr_no();

                int ret = 0;

                ret = jd_kw_openAPI.SendOrder("매수주문", l_scr_no, g_accnt_no, 1, l_jongmok_cd, l_buy_ord_stock_cnt, l_buy_price, "00", "");

                if(ret == 0) {
                    write_msg_log("매수주문 SendOrder() 호출 성공\n", 0);
                    write_msg_log("종목코드 : [" + l_jongmok_cd + "]\n", 0);
                } else {
                    write_msg_log("매수주문 SendOrder() 호출 실패\n", 0);
                    write_msg_log("i_jongmok_cd : [" + l_jongmok_cd + "]\n", 0);
                }

                delay(200);

                for(;;) {
                    if(g_flag_3 == 1) {
                        delay(200);
                        jd_kw_openAPI.DisconnectRealData(l_scr_no);
                        break;
                    }
                    else {
                        write_msg_log("'매수주문' 완료 대기중 .. \n", 0);
                        delay(200);
                        break;
                    }
                }

                jd_kw_openAPI.DisconnectRealData(l_scr_no);
                delay(1000);
            }

            reader.Close();
            conn.Close();
        }

        public int get_own_stock_cnt(string i_jongmok_cd) {
            OracleCommand cmd = null;
            OracleConnection conn = null;
            String sql = null;
            OracleDataReader reader = null;

            int l_own_stock_cnt = 0;

            conn = null;
            conn = connect_db();

            sql = null;
            cmd = null;
            reader = null;

            cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;

            //sql = @"SELECT NVL(MAX(OWN_STOCK_CNT), 0) OWN_STOCK_CNT FROM TB_ACCNT_INFO WHERE USER_ID = " + "'" + g_user_id+ "'" + 
            //" AND JONGMOK_CD = " + "'" + i_jongmok_cd + "'" + 
            //" AND ACCNT_NO = " + "'" + g_accnt_no + "'" + 
            //" AND REF_DT = TO_CHAR(SYSDATE, 'YYYYMMDD')";

            sql = @"SELECT NVL(MAX(OWN_STOCK_CNT), 0) OWN_STOCK_CNT FROM TB_ACCNT_INFO WHERE USER_ID = :g_user_id" +
            " AND JONGMOK_CD = :i_jongmok_cd"+
            " AND ACCNT_NO = :g_accnt_no" +
            " AND REF_DT = TO_CHAR(SYSDATE, 'YYYYMMDD')";

            cmd.CommandText = sql;
            cmd.Parameters.Add("i_jongmok_cd", i_jongmok_cd);
            cmd.Parameters.Add("g_accnt_no", g_accnt_no);

            reader = cmd.ExecuteReader();
            reader.Read();

            l_own_stock_cnt = int.Parse(reader[0].ToString());

            reader.Close();
            conn.Close();

            return l_own_stock_cnt;
        }

        public string get_buy_not_chegyul_yn(string i_jongmok_cd) {
            OracleCommand cmd = null;
            OracleConnection conn = null;
            String sql = null;
            OracleDataReader reader = null;

            int l_buy_not_chegyul_ord_stock_cnt = 0;
            string l_buy_not_chegyul_yn = null;

            conn = null;
            conn = connect_db();

            sql = null;
            cmd = null;
            reader = null;

            cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;

            //sql = @"select nvl(sum(ord_stock_cnt - CHEGYUL_STOCK_CNT), 0) buy_not_chegyul_ord_stock_cnt from " +
            //    "(select ord_stock_cnt ord_stock_cnt, (select nvl(max(b.CHEGYUL_STOCK_CNT), 0) CHEGYUL_STOCK_CNT " +
            //    "from tb_chegyul_lst b " +
            //    "where b.user_id = a.user_id " +
            //    "and b.accnt_no = a.accnt_no " +
            //    "and b.ref_dt = a.ref_dt " +
            //    "and b.jongmok_cd = a.jongmok_cd " +
            //    "and b.ord_gb = a.ord_gb " +
            //    "and b.ord_no = a.ord_no " +
            //    ") CHEGYUL_STOCK_CNT " +
            //    "from TB_ORD_LST a where a.ref_dt = TO_CHAR(SYSDATE, 'YYYYMMDD') " +
            //    "and a.ACCNT_NO = " + "'" + g_accnt_no + "'" +
            //    " and a.jongmok_cd = " + "'" + i_jongmok_cd + "'" +
            //    " and a.ord_gb = '2' " +
            //    " and a.org_ord_no = '0000000' " +
            //    " and not exists ( select '1' " +
            //    "from TB_ORD_LST b " +
            //    "where b.user_id = a.user_id " +
            //    "and b.accnt_no = a.accnt_no " +
            //    "and b.ref_dt = a.ref_dt " +
            //    "and b.jongmok_cd = a.jongmok_cd " +
            //    "and b.ord_gb = a.ord_gb " +
            //    "and b.org_ord_no = a.ord_no " +
            //    ") " + ") X";

            sql = @"select nvl(sum(ord_stock_cnt - CHEGYUL_STOCK_CNT), 0) buy_not_chegyul_ord_stock_cnt from " +
                "(select ord_stock_cnt ord_stock_cnt, (select nvl(max(b.CHEGYUL_STOCK_CNT), 0) CHEGYUL_STOCK_CNT " +
                "from tb_chegyul_lst b " +
                "where b.user_id = a.user_id " +
                "and b.accnt_no = a.accnt_no " +
                "and b.ref_dt = a.ref_dt " +
                "and b.jongmok_cd = a.jongmok_cd " +
                "and b.ord_gb = a.ord_gb " +
                "and b.ord_no = a.ord_no " +
                ") CHEGYUL_STOCK_CNT " +
                "from TB_ORD_LST a where a.ref_dt = TO_CHAR(SYSDATE, 'YYYYMMDD') " +
                "and a.ACCNT_NO = :g_accnt_no" +
                " and a.jongmok_cd = :i_jongmok_cd" +
                " and a.ord_gb = '2' " +
                " and a.org_ord_no = '0000000' " +
                " and not exists ( select '1' " +
                "from TB_ORD_LST b " +
                "where b.user_id = a.user_id " +
                "and b.accnt_no = a.accnt_no " +
                "and b.ref_dt = a.ref_dt " +
                "and b.jongmok_cd = a.jongmok_cd " +
                "and b.ord_gb = a.ord_gb " +
                "and b.org_ord_no = a.ord_no " +
                ") " + ") X";

            cmd.CommandText = sql;
            cmd.Parameters.Add("g_accnt_no", g_accnt_no);
            cmd.Parameters.Add("i_jongmok_cd", i_jongmok_cd);

            reader = cmd.ExecuteReader();
            reader.Read();

            l_buy_not_chegyul_ord_stock_cnt = int.Parse(reader[0].ToString());
            reader.Close();
            conn.Close();

            if(l_buy_not_chegyul_ord_stock_cnt > 0)
            {
                l_buy_not_chegyul_yn = "Y";
            } else
            {
                l_buy_not_chegyul_yn = "N";
            }

            return l_buy_not_chegyul_yn;
        }

        public int get_sell_not_chegyul_ord_stock_cnt(string i_jongmok_cd) // 미체결 매도주문 주식수 가져오기 메서드
        {
            OracleCommand cmd = null;
            OracleConnection conn = null;
            String sql = null;
            OracleDataReader reader = null;

            int l_sell_not_chegyul_ord_stock_cnt = 0;

            conn = null;
            conn = connect_db();

            sql = null;
            cmd = null;
            reader = null;

            cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;

            // 주문 내역과 체결내역 테이블 조회
            //sql = @"SELECT nvl(sum(ord_stock_cnt - CHEGYUL_STOCK_CNT), 0) sell_not_chegyul_ord_stock_cnt " +
            //        "FROM (" +
            //        "    SELECT ord_stock_cnt ord_stock_cnt, " +
            //        "    (" +
            //        "        select nvl(maax(b.CHEGYUL_STOCK_CNT), 0) CHEGYUL_STOCK_CNT " +
            //        "        FROM tb_chegyul_lst b" +
            //        "        WHERE b.user_id = a.user_id" +
            //        "        and b.accnt_no = a.accnt_no" +
            //        "        and b.ref_dt = a.ref_dt" +
            //        "        and b.jongmok_cd = a.jongmok_cd" +
            //        "        and b.ord_gb = a.ord_gb" +
            //        "        and b.ord_no = a.ord_no" +
            //        "    ) CHEGYUL_STOCK_CNT" +
            //        "    from TB_ORD_LST a" +
            //        "    where a.ref_dt = TO_CHAR(SYSDATE, 'YYYYMMDD')" +
            //        "    and a.user_id = " + "'" + g_user_id + "'" +
            //        "    and a.jongmok_cd = " + "'" + i_jongmok_cd + "'" +
            //        "    and a.ACCNT_NO = " + "'" + g_accnt_no + "'" +
            //        "    and a.ord_gb = '1'" +
            //        "    and a.org_ord_no = '0000000'" +
            //        "    and not exists (" +
            //        "        select '1' " +
            //        "        from TB_ORD_LST b" +
            //        "        where b.user_id = a.user_id" +
            //        "        and b.accnt_no = a.accnt_no" +
            //        "        and b.ref_dt = a.ref_dt" +
            //        "        and b.jongmok_cd = a.jongmok_cd" +
            //        "        and b.ord_gb = a.ord_gb" +
            //        "        and b.org_ord_no = a.ord_no" +
            //        "    )" +
            //        ")";

            sql = @"SELECT nvl(sum(ord_stock_cnt - CHEGYUL_STOCK_CNT), 0) sell_not_chegyul_ord_stock_cnt " +
                    "FROM (" +
                    "    SELECT ord_stock_cnt ord_stock_cnt, " +
                    "    (" +
                    "        select nvl(maax(b.CHEGYUL_STOCK_CNT), 0) CHEGYUL_STOCK_CNT " +
                    "        FROM tb_chegyul_lst b" +
                    "        WHERE b.user_id = a.user_id" +
                    "        and b.accnt_no = a.accnt_no" +
                    "        and b.ref_dt = a.ref_dt" +
                    "        and b.jongmok_cd = a.jongmok_cd" +
                    "        and b.ord_gb = a.ord_gb" +
                    "        and b.ord_no = a.ord_no" +
                    "    ) CHEGYUL_STOCK_CNT" +
                    "    from TB_ORD_LST a" +
                    "    where a.ref_dt = TO_CHAR(SYSDATE, 'YYYYMMDD')" +
                    "    and a.user_id = :g_user_id "+
                    "    and a.jongmok_cd = :i_jongmok_cd " +
                    "    and a.ACCNT_NO = :g_accnt_no"+
                    "    and a.ord_gb = '1'" +
                    "    and a.org_ord_no = '0000000'" +
                    "    and not exists (" +
                    "        select '1' " +
                    "        from TB_ORD_LST b" +
                    "        where b.user_id = a.user_id" +
                    "        and b.accnt_no = a.accnt_no" +
                    "        and b.ref_dt = a.ref_dt" +
                    "        and b.jongmok_cd = a.jongmok_cd" +
                    "        and b.ord_gb = a.ord_gb" +
                    "        and b.org_ord_no = a.ord_no" +
                    "    )" +
                    ")";

            cmd.CommandText = sql;
            cmd.Parameters.Add("g_user_id", g_user_id);
            cmd.Parameters.Add("i_jongmok_cd", i_jongmok_cd);
            cmd.Parameters.Add("g_accnt_no", g_accnt_no);


            reader = cmd.ExecuteReader();
            reader.Read();

            l_sell_not_chegyul_ord_stock_cnt = int.Parse(reader[0].ToString()); // 미체결 매도주문 주식수 가져오기
            reader.Close();
            conn.Close();

            return l_sell_not_chegyul_ord_stock_cnt;
        }

        public void sell_canc_ord(string i_jongmok_cd)
        {
            OracleCommand cmd = null;
            OracleConnection conn = null;
            String sql = null;
            OracleDataReader reader = null;

            string l_rid = null;
            string l_jongmok_cd = null;
            int l_ord_stock_cnt = 0;
            int l_ord_price = 0;
            string l_ord_no = null;
            string l_org_ord_no = null;

            conn = null;
            conn = connect_db();

            sql = null;
            cmd = null;
            reader = null;

            cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;

            //sql = @"select rowid rid, jongmok_cd, (ord_stock_cnt - " +
            //        "    (select nvl(max(b.CHEGYUL_STOCK_CNT), 0) CHEGYUL_STOCK_CNT " +
            //        "     from tb_chegyul_lst b" +
            //        "     where b.user_id = a.user_id" +
            //        "     and b.accnt_no = a.accnt_no" +
            //        "     and b.ref_dt = a.ref_dt" +
            //        "     and b.jongmok_cd = a.jongmok_cd" +
            //        "     and b.ord_gb = a.ord_gb" +
            //        "     and b.ord_no = a.ord_no" +
            //        "    )" +
            //        ") sell_not_chegyul_ord_stock_cnt, " +
            //        "    ord_price," +
            //        "    ord_no," +
            //        "    org_ord_no" +
            //        "    from TB_ORD_LST a" +
            //        "    where a.ref_dt = TO_CHAR(SYSDATE, 'YYYYMMDD')" +
            //        "    and a.user_id = " + "'" + g_user_id + "'" +
            //        "    and a.accnt_no = " + "'" + g_accnt_no + "'" +
            //        "    and a.jongmok_cd = " + "'" + i_jongmok_cd + "'" +
            //        "    and a.ord_gb = '1'" +
            //        "    and a.org_ord_no = '0000000'" +
            //        "    and not exists (" +
            //        "        select '1'" +
            //        "        from TB_ORD_LST b" +
            //        "        where b.user_id = a.user_id" +
            //        "        and b.accnt_no = a.accnt_no" +
            //        "        and b.ref_dt = a.ref_dt" +
            //        "        and b.jongmok_cd = a.jongmok_cd" +
            //        "        and b.ord_gb = a.ord_gb" +
            //        "        and b.org_ord_no = a.ord_no" +
            //        "   )";

            sql = @"select rowid rid, jongmok_cd, (ord_stock_cnt - " +
                    "    (select nvl(max(b.CHEGYUL_STOCK_CNT), 0) CHEGYUL_STOCK_CNT " +
                    "     from tb_chegyul_lst b" +
                    "     where b.user_id = a.user_id" +
                    "     and b.accnt_no = a.accnt_no" +
                    "     and b.ref_dt = a.ref_dt" +
                    "     and b.jongmok_cd = a.jongmok_cd" +
                    "     and b.ord_gb = a.ord_gb" +
                    "     and b.ord_no = a.ord_no" +
                    "    )" +
                    ") sell_not_chegyul_ord_stock_cnt, " +
                    "    ord_price," +
                    "    ord_no," +
                    "    org_ord_no" +
                    "    from TB_ORD_LST a" +
                    "    where a.ref_dt = TO_CHAR(SYSDATE, 'YYYYMMDD')" +
                    "    and a.user_id = :g_user_id " +
                    "    and a.accnt_no = :g_accnt_no " +
                    "    and a.jongmok_cd = :i_jongmok_cd " +
                    "    and a.ord_gb = '1'" +
                    "    and a.org_ord_no = '0000000'" +
                    "    and not exists (" +
                    "        select '1'" +
                    "        from TB_ORD_LST b" +
                    "        where b.user_id = a.user_id" +
                    "        and b.accnt_no = a.accnt_no" +
                    "        and b.ref_dt = a.ref_dt" +
                    "        and b.jongmok_cd = a.jongmok_cd" +
                    "        and b.ord_gb = a.ord_gb" +
                    "        and b.org_ord_no = a.ord_no" +
                    "   )";

            cmd.CommandText = sql;
            cmd.Parameters.Add("g_user_id", g_user_id);
            cmd.Parameters.Add("g_accnt_no", g_accnt_no);
            cmd.Parameters.Add("i_jongmok_cd", i_jongmok_cd);



            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                l_rid = "";
                l_jongmok_cd = "";
                l_ord_stock_cnt = 0;
                l_ord_price = 0;
                l_ord_no = "";
                l_org_ord_no = "";

                l_rid = reader[0].ToString().Trim();
                l_jongmok_cd = reader[1].ToString().Trim();
                l_ord_stock_cnt = int.Parse(reader[2].ToString().Trim());
                l_ord_price = int.Parse(reader[3].ToString().Trim());
                l_ord_no = reader[4].ToString().Trim();
                l_org_ord_no = reader[5].ToString().Trim();

                g_flag_5 = 0;
                g_rqname = "매도취소주문";

                String l_scr_no = null;
                l_scr_no = "";
                l_scr_no = get_scr_no();

                int ret = 0;
                // 매도 취소 주문 요청
                ret = jd_kw_openAPI.SendOrder("매도취소주문", l_scr_no, g_accnt_no, 4, l_jongmok_cd, l_ord_stock_cnt, 0, "03", l_ord_no);

                if (ret == 0)
                {
                    write_msg_log("매도취소주문 Sendord() 호출 성공\n", 0);
                    write_msg_log("종목코드 [" + l_jongmok_cd + "]\n", 0);

                }
                else
                {
                    write_msg_log("매도취소주문 Sendord() 호출 실패\n", 0);
                    write_msg_log("i_jongmok_cd : [" + l_jongmok_cd + "]\n", 0);

                }

                delay(200);

                for (; ; )
                {
                    if (g_flag_5 == 1)
                    {
                        delay(200);
                        jd_kw_openAPI.DisconnectRealData(l_scr_no);
                        break;
                    }
                    else
                    {
                        write_msg_log("매도취소주문 완료 대기중 .. \n", 0);
                        delay(200);
                        break;
                    }
                }

                jd_kw_openAPI.DisconnectRealData(l_scr_no);
                delay(1000);
            }

            reader.Close();
            conn.Close();
        }

        private void axKHOpenAPI1_OnReceiveTrData(object sender, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveTrDataEvent e)
        {
            if (g_rqname.CompareTo(e.sRQName) == 0)
            {
                ; // 다음으로 진행
            }
            else // 요청한 요청명과 OpenAPI 로 부터 응답받은 요청명이 같지 않다면
            {
                write_err_log("요청한 TR : [" + g_rqname + "]\n", 0);
                write_err_log("응답받은 TR : [" + e.sRQName + "]\n", 0);

                switch (g_rqname)
                {
                    case "증거금세부내역조회요청":
                        g_flag_1 = 1;
                        break;
                    case "계좌평가현황요청":
                        g_flag_2 = 1;
                        break;
                    case "호가조회":
                        g_flag_7 = 1;

                        break;
                    case "현재가조회":
                        g_flag_6 = 1; // g_flag_6를 1로 설정하여 요청하는 쪽이 무한루프에 빠지지 않게함.
                        break;
                    default:
                        break;
                }
                return;
            }
            if(e.sRQName == "증거금세부내역조회요청")
            {
                g_ord_amt_possible = int.Parse(jd_kw_openAPI.CommGetData(e.sTrCode, "", e.sRQName, 0, "100주문가능금액").Trim());
                jd_kw_openAPI.DisconnectRealData(e.sScrNo);
                g_flag_1 = 1;
            }

            if(e.sRQName == "계좌평가현황요청")
            {
                int repeat_cnt = 0;
                int ii = 0;

                String user_id = null;
                String jongmok_cd = null;
                String jongmok_nm = null;

                int own_stock_cnt = 0;
                int buy_price = 0;
                int own_amt = 0;

                repeat_cnt = jd_kw_openAPI.GetRepeatCnt(e.sTrCode, e.sRQName);

                write_msg_log("TB_ACCNT_INFO 테이블 설정 시작\n", 0);
                write_msg_log("보유종목수 : " + repeat_cnt.ToString() + "\n", 0);
                write_msg_log("e.sTrCode : " + e.sTrCode.ToString() + "\n", 0);
                write_msg_log("e.sRQName : " + e.sRQName.ToString() + "\n", 0);


                for (ii = 0;ii < repeat_cnt;ii++)
                {
                    user_id = "";
                    jongmok_cd = "";
                    own_stock_cnt = 0;
                    buy_price = 0;
                    own_amt = 0;

                    user_id = g_user_id;
                    jongmok_cd = jd_kw_openAPI.CommGetData(e.sTrCode, "", e.sRQName, ii, "종목코드").Trim().Substring(1, 6);
                    jongmok_nm = jd_kw_openAPI.CommGetData(e.sTrCode, "", e.sRQName, ii, "종목명").Trim();
                    own_stock_cnt = int.Parse(jd_kw_openAPI.CommGetData(e.sTrCode, "", e.sRQName, ii, "보유수량").Trim());
                    buy_price = int.Parse(jd_kw_openAPI.CommGetData(e.sTrCode, "", e.sRQName, ii, "평균단가").Trim());
                    own_amt = int.Parse(jd_kw_openAPI.CommGetData(e.sTrCode, "", e.sRQName, ii, "매입금액").Trim());

                    write_msg_log("종목코드 : " + jongmok_cd + "\n", 0);
                    write_msg_log("종목명 : " + jongmok_nm + "\n", 0);
                    write_msg_log("보유주식수 : " + own_stock_cnt.ToString() + "\n", 0);

                    if(own_stock_cnt == 0)
                    {
                        continue;
                    }

                    insert_tb_accnt_info(jongmok_cd, jongmok_nm, buy_price, own_stock_cnt, own_amt);



                }


                write_msg_log("TB_ACCNT_INFO 테이블 설정 완료\n", 0);
                jd_kw_openAPI.DisconnectRealData(e.sScrNo);

                if (e.sPrevNext.Length == 0)
                {
                    g_is_next = 0;
                }
                else
                {
                    g_is_next = int.Parse(e.sPrevNext);
                }
                g_flag_2 = 1;
            }

            if(e.sRQName == "호가조회")
            {
                int cnt = 0;
                int ii = 0;
                int l_buy_hoga = 0;

                cnt = jd_kw_openAPI.GetRepeatCnt(e.sTrCode, e.sRQName);

                for(ii = 0; ii < cnt; ii++)
                {
                    l_buy_hoga = int.Parse(jd_kw_openAPI.GetCommData(e.sTrCode, "", ii, "매수최우선호가").Trim());
                    l_buy_hoga = System.Math.Abs(l_buy_hoga);
                }

                g_buy_hoga = l_buy_hoga;

                jd_kw_openAPI.DisconnectRealData(e.sScrNo);
                g_flag_7 = 1;
            }

            if(e.sRQName == "현재가조회")
            {
                g_cur_price = int.Parse(jd_kw_openAPI.CommGetData(e.sTrCode, "", e.sRQName, 0, "현재가").Trim());
                g_cur_price = System.Math.Abs(g_cur_price);
                jd_kw_openAPI.DisconnectRealData(e.sScrNo);
                g_flag_6 = 1;
            }
        } // onReceiveTrData 종료

        public void insert_tb_chegyul_lst(string i_ref_dt, String i_jongmok_cd, String i_jongmok_nm, String i_chegyul_gb, int i_chegyul_no, 
            int i_chegyul_price, int i_chegyul_stock_cnt, int i_chegyul_amt, String i_chegyul_dtm, String i_ord_no, String i_org_ord_no)
        // 체결 내역
        // 저장 메서드
        {
            OracleCommand cmd = null;
            OracleConnection conn = null;
            String l_sql = null;

            l_sql = null;

            cmd = null;
            conn = null;
            conn = connect_db();

            cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;

            //l_sql = @"INSERT INTO tb_chegyul_lst VALUES (" +
            //    "'" + g_user_id + "'" + "," +
            //    "'" + g_accnt_no + "'" + "," +
            //    "'" + i_ref_dt + "'" + "," +
            //    "'" + i_jongmok_cd + "'" + "," +
            //    "'" + i_jongmok_nm + "'" + "," +
            //    "'" + i_chegyul_gb + "'" + "," +
            //    "'" + i_ord_no + "'" + "," +
            //    "'" + i_chegyul_gb + "'" + "," +
            //    i_chegyul_no + "," +
            //    i_chegyul_price + "," +
            //    i_chegyul_stock_cnt + "," +
            //    i_chegyul_amt + "," +
            //    "'" + i_chegyul_dtm + "'" + "," +
            //    "'ats'" + "," +
            //    "SYSDATE" + "," +
            //    "null" + "," +
            //    "null" + ") ";

            l_sql = @"INSERT INTO tb_chegyul_lst VALUES (" +
                ":g_user_id, :g_accnt_no, :i_ref_dt, :i_jongmok_cd, :i_jongmok_nm, :i_chegyul_gb, :i_ord_no, " +
                ":i_chegyul_gb, :i_chegyul_no, :i_chegyul_price, :i_chegyul_stock_cnt, :i_chegyul_amt, :i_chegyul_dtm, " + 
                "'ats'" + "," +
                "SYSDATE" + "," +
                "null" + "," +
                "null" + ") ";

            cmd.CommandText = l_sql;
            cmd.Parameters.Add("g_user_id", g_user_id);
            cmd.Parameters.Add("g_accnt_no", g_accnt_no);
            cmd.Parameters.Add("i_ref_dt", i_ref_dt);
            cmd.Parameters.Add("i_jongmok_cd", i_jongmok_cd);
            cmd.Parameters.Add("i_jongmok_nm", i_jongmok_nm);
            cmd.Parameters.Add("i_chegyul_gb", i_chegyul_gb);
            cmd.Parameters.Add("i_ord_no", i_ord_no);
            cmd.Parameters.Add("i_chegyul_no", i_chegyul_no);
            cmd.Parameters.Add("i_chegyul_price", i_chegyul_price);
            cmd.Parameters.Add("i_chegyul_stock_cnt", i_chegyul_stock_cnt);
            cmd.Parameters.Add("i_chegyul_amt", i_chegyul_amt);
            cmd.Parameters.Add("i_chegyul_dtm", i_chegyul_dtm);


            try
            {
                cmd.ExecuteNonQuery();
            } catch(Exception ex)
            {
                write_err_log("insert tb_chegyul_lst ex.Message : [" + ex.Message + "] \n", 0);
            }

            conn.Close();

        }

        public void sell_ord_first()
        {
            OracleCommand cmd = null;
            OracleConnection conn = null;
            String sql = null;
            OracleDataReader reader = null;

            string l_jongmok_cd = null;
            int l_buy_price = 0;
            int l_own_stock_cnt = 0;
            int l_target_price = 0;

            conn = null;
            conn = connect_db();

            sql = null;
            cmd = null;
            reader = null;

            cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;

            // TB_ACCNT_INFO와 TB_TRD_JONGMOK 테이블을 조인하여 매도대상 종목 조회

            //sql = @"SELECT " +
            //    "   A.JONGMOK_CD, " +
            //    "   A.BUY_PRICE, " +
            //    "   A.OWN_STOCK_CNT, " +
            //    "   B.TARGET_PRICE " +
            //    " FROM TB_ACCNT_INFO A, " +
            //    "   TB_TRD_JONGMOK B " +
            //    " WHERE A.USER_ID = " + "'" + g_user_id + "'" +
            //    " AND A.ACCNT_NO = " + "'" + g_accnt_no + "'" +
            //    " AND A.REF_DT = TO_CHAR(SYSDATE, 'YYYYMMDD') " +
            //    " AND A.USER_ID = B.USER_ID " +
            //    " AND A.JONGMOK_CD = B.JONGMOK_CD " +
            //    " AND B.SELL_TRD_YN = 'Y' AND A.OWN_STOCK_CNT > 0";

            sql = @"SELECT " +
                "   A.JONGMOK_CD, " +
                "   A.BUY_PRICE, " +
                "   A.OWN_STOCK_CNT, " +
                "   B.TARGET_PRICE " +
                " FROM TB_ACCNT_INFO A, " +
                "   TB_TRD_JONGMOK B " +
                " WHERE A.USER_ID = :g_user_id " +
                " AND A.ACCNT_NO = :g_accnt_no " +
                " AND A.REF_DT = TO_CHAR(SYSDATE, 'YYYYMMDD') " +
                " AND A.USER_ID = B.USER_ID " +
                " AND A.JONGMOK_CD = B.JONGMOK_CD " +
                " AND B.SELL_TRD_YN = 'Y' AND A.OWN_STOCK_CNT > 0";

            cmd.CommandText = sql;
            cmd.Parameters.Add("g_user_id", g_user_id);
            cmd.Parameters.Add("g_accnt_no", g_accnt_no);

            reader = cmd.ExecuteReader();

            while(reader.Read())
            {
                l_jongmok_cd = "";
                l_buy_price = 0;
                l_own_stock_cnt = 0;
                l_target_price = 0;

                l_jongmok_cd = reader[0].ToString().Trim();
                l_buy_price = int.Parse(reader[1].ToString().Trim());
                l_own_stock_cnt = int.Parse(reader[2].ToString().Trim());
                l_target_price = int.Parse(reader[3].ToString().Trim());

                write_msg_log("종목 코드 : [" + l_jongmok_cd + "]\n", 0);
                write_msg_log("매입가 : [" + l_buy_price.ToString() + "]\n", 0);
                write_msg_log("보유 주식 수 : [" + l_own_stock_cnt.ToString() + "]\n", 0);
                write_msg_log("목표가 : [" + l_target_price.ToString() + "]\n", 0);

                int l_new_target_price = 0;
                l_new_target_price = get_hoga_unit_price(l_target_price, l_jongmok_cd, 0);

                g_flag_4 = 0;
                g_rqname = "매도주문";
                String l_scr_no = null;
                l_scr_no = "";
                l_scr_no = get_scr_no();

                int ret = 0;

                // 매도 주문 요청
                ret = jd_kw_openAPI.SendOrder("매도주문", l_scr_no, g_accnt_no, 2, l_jongmok_cd, l_own_stock_cnt, l_new_target_price, "00", "");

                if(ret == 0)
                {
                    write_msg_log("매도주문 Sendord() 호출 성공 \n", 0);
                    write_msg_log("종목코드 : [" + l_jongmok_cd + "]\n", 0);
                } else
                {
                    write_msg_log("매도주문 Sendord() 호출 실패 \n", 0);
                    write_msg_log("i_jongmok_cd : [ " + l_jongmok_cd + "]\n", 0);
                }

                delay(200);

                for(; ; )
                {
                    if(g_flag_4 == 1)
                    {
                        delay(200);
                        jd_kw_openAPI.DisconnectRealData(l_scr_no);
                        break;
                    } else
                    {
                        write_msg_log("'매도주문' 완료 대기중 .. \n", 0);
                        delay(200);
                        break;
                    }
                }

                jd_kw_openAPI.DisconnectRealData(l_scr_no);

            } // while (reader.Read()) 종료

            reader.Close();
            conn.Close();

        } // end of sell_ord_first

        public int get_hoga_unit_price(int i_price, String i_jongmok_cd, int i_hoga_unit_jump) // 호가 가격단위 가져오기 메서드
        {
            int l_market_type;
            int l_rest;

            l_market_type = 0;

            try
            {
                l_market_type = int.Parse(jd_kw_openAPI.GetMarketType(i_jongmok_cd).ToString());
            } catch(Exception ex)
            {
                write_err_log("get_hoga_unit_price() ex.Message : [" + ex.Message + "]\n", 0);
            }

            if(i_price < 1000)
            {
                return i_price + (i_hoga_unit_jump * 1);
            } else if(i_price >= 1000 && i_price < 5000)
            {
                l_rest = i_price % 5;
                if(l_rest == 0)
                {
                    return i_price + (i_hoga_unit_jump * 5);
                } else if(l_rest < 3)
                {
                    return (i_price - l_rest) + (i_hoga_unit_jump * 5);
                } else
                {
                    return (i_price + (5 - l_rest)) + (i_hoga_unit_jump * 5);
                }
            } else if(i_price >= 5000 && i_price < 10000)
            {
                l_rest = i_price % 10;
                if(l_rest == 0)
                {
                    return i_price + (i_hoga_unit_jump * 10);
                } else if(l_rest < 5)
                {
                    return (i_price - l_rest) + (i_hoga_unit_jump * 10);
                } else
                {
                    return (i_price + (10 - l_rest)) + (i_hoga_unit_jump * 10);
                }
            } else if(i_price >= 10000 && i_price < 50000)
            {
                l_rest = i_price % 50;
                if(l_rest == 0)
                {
                    return i_price + (i_hoga_unit_jump * 50);
                } else if(l_rest < 25)
                {
                    return (i_price - l_rest) + (i_hoga_unit_jump * 50);
                } else
                {
                    return (i_price + (50 - l_rest)) + (i_hoga_unit_jump * 50);
                }
            } else if(i_price >= 50000 && i_price < 100000)
            {
                l_rest = i_price % 100;
                if(l_rest == 0)
                {
                    return i_price + (i_hoga_unit_jump * 100);
                } else if(l_rest < 50)
                {
                    return (i_price - l_rest) + (i_hoga_unit_jump * 100);
                } else
                {
                    return (i_price + (100 - l_rest)) + (i_hoga_unit_jump * 100);
                }
            } else if(i_price >= 100000 && i_price < 500000)
            {
                if(l_market_type == 10)
                {
                    l_rest = i_price % 100;
                    if(l_rest == 0)
                    {
                        return i_price + (i_hoga_unit_jump * 100);
                    } else if(l_rest < 50)
                    {
                        return (i_price - l_rest) + (i_hoga_unit_jump * 100);
                    } else
                    {
                        return (i_price + (100 - l_rest)) + (i_hoga_unit_jump * 100);
                    }
                } else
                {
                    l_rest = i_price % 500;
                    if(l_rest == 0)
                    {
                        return i_price + (i_hoga_unit_jump * 500);
                    } else if(l_rest < 250)
                    {
                        return (i_price - l_rest) + (i_hoga_unit_jump * 500);
                    } else
                    {
                        return (i_price + (500 - l_rest)) + (i_hoga_unit_jump * 500);
                    }
                }
            } else if(i_price >= 500000)
            {
                if(l_market_type == 10)
                {
                    l_rest = i_price % 100;
                    if(l_rest == 0)
                    {
                        return i_price + (i_hoga_unit_jump * 100);
                    } else if(l_rest < 50)
                    {
                        return (i_price - l_rest) + (i_hoga_unit_jump * 100);
                    } else
                    {
                        return (i_price + (100 - l_rest) + (i_hoga_unit_jump * 100));
                    }
                } else
                {
                    l_rest = i_price % 1000;
                    if(l_rest == 0)
                    {
                        return i_price + (i_hoga_unit_jump * 1000);
                    } else if(l_rest < 500)
                    {
                        return (i_price - l_rest) + (i_hoga_unit_jump * 1000);
                    } else
                    {
                        return (i_price + (1000 - l_rest)) + (i_hoga_unit_jump * 1000);
                    }
                }
            }

            return 0;
        }

        public void real_sell_ord()
        {
            OracleCommand cmd = null;
            OracleConnection conn = null;
            String sql = null;
            OracleDataReader reader = null;

            string l_jongmok_cd = null;
            int l_target_price = 0;
            int l_own_stock_cnt = 0;

            write_msg_log("real_sell_ord 시작\n", 0);

            conn = null;
            conn = connect_db();

            sql = null;
            cmd = null;
            reader = null;

            cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;

            //sql = @"SELECT " +
            //    "A.JONGMOK_CD, " +
            //    "A.TARGET_PRICE, " +
            //    "B.OWN_STOCK_CNT " +
            //    "FROM " +
            //    "TB_TRD_JONGMOK A, " +
            //    "TB_ACCNT_INFO B " +
            //    "WHERE A.USER_ID = " + "'" + g_user_id + "'" +
            //    " AND A.JONGMOK_CD = B.JONGMOK_CD " +
            //    " AND B.ACCNT_NO " + "'" + g_accnt_no + "'" +
            //    " AND B.REF_DT = TO_CHAR(SYSDATE, 'YYYYMMDD') " +
            //    "AND A.SELL_TRD_YN = 'Y' AND B.OWN_STOCK_CNT > 0";

            sql = @"SELECT " +
               "A.JONGMOK_CD, " +
               "A.TARGET_PRICE, " +
               "B.OWN_STOCK_CNT " +
               "FROM " +
               "TB_TRD_JONGMOK A, " +
               "TB_ACCNT_INFO B " +
               "WHERE A.USER_ID = :g_user_id " +
               " AND A.JONGMOK_CD = B.JONGMOK_CD " +
               " AND B.ACCNT_NO :g_accnt_no "+
               " AND B.REF_DT = TO_CHAR(SYSDATE, 'YYYYMMDD') " +
               "AND A.SELL_TRD_YN = 'Y' AND B.OWN_STOCK_CNT > 0";

            cmd.CommandText = sql;
            cmd.Parameters.Add("g_user_id", g_user_id);
            cmd.Parameters.Add("g_accnt_no", g_accnt_no);

            reader = cmd.ExecuteReader();

            while(reader.Read())
            {
                l_jongmok_cd = "";
                l_target_price = 0;

                l_jongmok_cd = reader[0].ToString().Trim();
                l_target_price = int.Parse(reader[1].ToString().Trim());
                l_own_stock_cnt = int.Parse(reader[2].ToString().Trim());

                write_msg_log("종목 코드 : [" + l_jongmok_cd + "]\n", 0);
                write_msg_log("종목명 : [" + get_jongmok_nm(l_jongmok_cd) + "]\n", 0);
                write_msg_log("목표가: [" + l_target_price.ToString() + "]\n", 0);
                write_msg_log("보유 주식수: [" + l_own_stock_cnt.ToString() + "]\n", 0);

                int l_sell_not_chegyul_ord_stock_cnt = 0;
                l_sell_not_chegyul_ord_stock_cnt = get_sell_not_chegyul_ord_stock_cnt(l_jongmok_cd); // 미체결 매도주문 주식수 구하기

                if(l_sell_not_chegyul_ord_stock_cnt == l_own_stock_cnt) // 미체결 매도주문 주식수와 보유 주식수가 같으면 기 주문종목이므로 매도주문 하지 않음
                {
                    continue;
                }
                else // 미체결 매도주문 주식수와 보유주식수가 같지 않으면 아직 매도하지않은 종목임
                {
                    int l_sell_ord_stock_cnt_tmp = 0;
                    l_sell_not_chegyul_ord_stock_cnt = l_own_stock_cnt - l_sell_not_chegyul_ord_stock_cnt;
                    // 보유 주식수에서 미체결 매도주문 주식수를 빼서 매도주문 주식수를 구함
                    if(l_sell_ord_stock_cnt_tmp <= 0) // 매도대상 주식수가 0 이하라면 매도하지않음
                    {
                        continue;
                    }

                    int l_new_target_price = 0;
                    l_new_target_price = get_hoga_unit_price(l_target_price, l_jongmok_cd, 0);

                    // 매도 호가를 구함
                    g_flag_4 = 0;
                    g_rqname = "매도주문";

                    String l_scr_no = null;
                    l_scr_no = "";
                    l_scr_no = get_scr_no();

                    int ret = 0;

                    // 매도주문 요청

                    ret = jd_kw_openAPI.SendOrder("매도주문", l_scr_no, g_accnt_no, 2, l_jongmok_cd, l_sell_ord_stock_cnt_tmp, l_new_target_price, "00", "");

                    if(ret == 0)
                    {
                        write_msg_log("매도주문 Sendord() 호출 성공\n", 0);
                        write_msg_log("종목코드 : [" + l_jongmok_cd + "]\n", 0);
                    } else
                    {
                        write_msg_log("매도주문 Sendord() 호출 실패\n", 0);
                        write_msg_log("l_jongmok_cd : [" + l_jongmok_cd + "]\n", 0);
                    }

                    delay(200); // 0.2 delay

                    for(; ; )
                    {
                        if(g_flag_4 == 1)
                        {
                            delay(200);
                            jd_kw_openAPI.DisconnectRealData(l_scr_no);
                            break;
                        } else
                        {
                            write_msg_log("'매도주문' 완료대기중 ... \n", 0);
                            delay(200);
                            break;
                        }
                    }

                    jd_kw_openAPI.DisconnectRealData(l_scr_no);

                }

            } // while(reader.Read()) 종료

            reader.Close();
            conn.Close();

        }

        public void real_cut_loss_ord() // 실시간 손절주문 메서드 
        {
            OracleCommand cmd = null;
            OracleConnection conn = null;
            String sql = null;
            OracleDataReader reader = null;

            string l_jongmok_cd = null;
            int l_cut_loss_price = 0;
            int l_own_stock_cnt = 0;

            write_msg_log("real_cut_loss_ord 시작\n", 0);

            conn = null;
            conn = connect_db();

            sql = null;
            cmd = null;
            reader = null;

            cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;

            //sql = @"SELECT " +
            //    "A.JONGMOK_CD, " +
            //    "A.CUT_LOSS_PRICE, " +
            //    "B.OWN_STOCK_CNT " +
            //    "FROM " +
            //    " TB_TRD_JONGMOK A, " +
            //    " TB_ACCNT_INFO B " +
            //    "WHERE A.USER_ID = " + "'" + g_user_id + "'" +
            //    " AND A.JONGMOK_CD = B.JONGMOK_CD " +
            //    "AND B.ACCNT_NO " + "'" + g_accnt_no + "'" +
            //    " AND B.REF_DT = TO_CHAR(SYSDATE, 'YYYYMMDD') " +
            //    "AND A.SELL_TRD_YN = 'Y' AND B.OWN_STOCK_CNT > 0";

            sql = @"SELECT " +
                "A.JONGMOK_CD, " +
                "A.CUT_LOSS_PRICE, " +
                "B.OWN_STOCK_CNT " +
                "FROM " +
                " TB_TRD_JONGMOK A, " +
                " TB_ACCNT_INFO B " +
                "WHERE A.USER_ID = :g_user_id " +
                " AND A.JONGMOK_CD = B.JONGMOK_CD " +
                "AND B.ACCNT_NO = :g_accnt_no " +
                " AND B.REF_DT = TO_CHAR(SYSDATE, 'YYYYMMDD') " +
                "AND A.SELL_TRD_YN = 'Y' AND B.OWN_STOCK_CNT > 0";


            cmd.CommandText = sql;
            cmd.Parameters.Add("g_user_id", g_user_id);
            cmd.Parameters.Add("g_accnt_no", g_accnt_no);

            reader = cmd.ExecuteReader();

            while(reader.Read())
            {
                l_jongmok_cd = "";
                l_cut_loss_price = 0;

                l_jongmok_cd = reader[0].ToString().Trim();
                l_cut_loss_price = int.Parse(reader[1].ToString().Trim());
                l_own_stock_cnt = int.Parse(reader[2].ToString().Trim());

                write_msg_log("종목코드 : [" + l_jongmok_cd + "]\n", 0);
                write_msg_log("종목명 : [" + get_jongmok_nm(l_jongmok_cd) + "]\n", 0);
                write_msg_log("손절가 : [" + l_cut_loss_price.ToString() + "]\n", 0);
                write_msg_log("보유주식수 : [" + l_own_stock_cnt.ToString() + "]\n", 0);
            }

            int l_for_flag = 0;
            g_cur_price = 0;


            for(; ; )
            {
                g_rqname = "";
                g_rqname = "현재가 조회";
                g_flag_6 = 0;
                jd_kw_openAPI.SetInputValue("종목코드", l_jongmok_cd);

                string l_scr_no = null;
                l_scr_no = "";
                l_scr_no = get_scr_no();

                // 현재가 조회 요청
                jd_kw_openAPI.CommRqData(g_rqname, "opt10001", 0, l_scr_no);

                try
                {
                    int l_for_cnt = 0;
                    for(; ; )
                    {
                        if(g_flag_6 == 1)
                        {
                            delay(200);
                            jd_kw_openAPI.DisconnectRealData(l_scr_no);
                            l_for_flag = 1;
                            break;
                        } else
                        {
                            write_msg_log("'현재가 조회' 완료 대기중 ... \n", 0);
                            delay(200);
                            l_for_cnt++;
                            if(l_for_cnt == 5)
                            {
                                l_for_flag = 0;
                                break;
                            } else
                            {
                                continue;
                            }
                        }
                    }
                } catch(Exception ex)
                {
                    write_err_log("real_cut_loss_ord() 현재가 조회 ex.Message : [" + ex.Message + "]\n", 0);
                    
                }

                if(g_cur_price < l_cut_loss_price) // 현재가가 손절가 이탈 시
                {
                    write_msg_log("현재 가격이 손절 가격을 이탈\n", 0);

                    write_msg_log("sell_canc_ord() 시작\n", 0);

                    sell_canc_ord(l_jongmok_cd);
                    write_msg_log("sell_canc_ord() 완료\n", 0);
                    g_flag_4 = 0;
                    g_rqname = "매도주문";

                    l_scr_no = null;
                    l_scr_no = "";
                    l_scr_no = get_scr_no();

                    int ret = 0;

                    // 매도주문 요청
                    ret = jd_kw_openAPI.SendOrder("매도주문", l_scr_no, g_accnt_no, 2, l_jongmok_cd, l_own_stock_cnt, 0, "03", "");

                    if(ret == 0)
                    {
                        write_msg_log("매도주문 Sendord() 호출 성공\n", 0);
                        write_msg_log("종목코드 : [" + l_jongmok_cd + "]\n", 0);

                    } else
                    {
                        write_msg_log("매도주문 Sendord() 호출 실패\n", 0);
                        write_msg_log("i_jongmok_cd : [" + l_jongmok_cd + "]\n", 0);

                    }

                    delay(200);

                    for(; ; )
                    {
                        if(g_flag_4 == 1)
                        {
                            delay(200);
                            jd_kw_openAPI.DisconnectRealData(l_scr_no);
                            break;
                        } else
                        {
                            write_msg_log("'매도주문' 완료 대기 중 ... \n", 0);
                            delay(200);
                            break;
                        }
                    }
                    jd_kw_openAPI.DisconnectRealData(l_scr_no);
                    update_tb_trd_jongmok(l_jongmok_cd);
                }

                

                if(l_for_flag == 1)
                {
                    break;
                } else if(l_for_flag == 0)
                {
                    delay(200);
                    continue;
                }

                delay(200);


            }

            reader.Close();
            conn.Close();
        }

        private void update_tb_trd_jongmok(String i_jongmok_cd)
        {
            OracleCommand cmd = null;
            OracleConnection conn = null;
            String l_sql = null;

            l_sql = null;
            cmd = null;
            conn = null;
            conn = connect_db();
            

            cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;

            //l_sql = @"update TB_TRD_JONGMOK set buy_trd_yn = 'N', updt_dtm = SYSDATE, updt_id = 'ats' " +
            //    " WHERE user_id = " + "'" + g_user_id + "'" +
            //    " and jongmok_cd = " + "'" + i_jongmok_cd + "'";

            l_sql = @"update TB_TRD_JONGMOK set buy_trd_yn = 'N', updt_dtm = SYSDATE, updt_id = 'ats' " +
                " WHERE user_id = :g_user_id " +
                " and jongmok_cd = :i_jongmok_cd";

            cmd.CommandText = l_sql;
            cmd.Parameters.Add("g_user_id", g_user_id);
            cmd.Parameters.Add("i_jongmok_cd", i_jongmok_cd);

            try
            {
                cmd.ExecuteNonQuery();
            } catch(Exception ex)
            {
                write_err_log("update TB_TRD_JONGMOK ex.Message : [" + ex.Message + "]\n", 0);
            }

            conn.Close();
        }



        private void axKHOpenAPI1_OnReceiveMsg(object sender, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveMsgEvent e)
        {
            if(e.sRQName == "매수주문")
            {
                write_msg_log("\n=========매수주문 원장 응답정보 출력 시작==========\n", 0);
                write_msg_log("sScrNo : [" + e.sScrNo + "]" + "\n", 0);
                write_msg_log("sRQName : [" + e.sRQName + "]" + "\n", 0);
                write_msg_log("sTrCode : [" + e.sTrCode + "]" + "\n", 0);
                write_msg_log("sMsg : [" + e.sMsg + "]" + "\n", 0);

                write_msg_log("=========매수주문 원장 응답정보 출력 종료==========\n", 0);
                g_flag_3 = 1; // 매수주문 응답 완료 설정
            }

            if(e.sRQName == "매도주문")
            {
                write_msg_log("\n=========매도주문 원장 응답정보 출력 시작==========\n", 0);
                write_msg_log("sScrNo : [" + e.sScrNo + "]" + "\n", 0);
                write_msg_log("sRQName : [" + e.sRQName + "]" + "\n", 0);
                write_msg_log("sTrCode : [" + e.sTrCode + "]" + "\n", 0);
                write_msg_log("sMsg : [" + e.sMsg + "]" + "\n", 0);

                write_msg_log("=========매도주문 원장 응답정보 출력 종료==========\n", 0);
                g_flag_3 = 1; // 매도주문 응답 완료 설정
            }

            if(e.sRQName == "매도취소주문")
            {
                write_msg_log("\n=========매도취소주문 원장 응답정보 출력 시작==========\n", 0);
                write_msg_log("sScrNo : [" + e.sScrNo + "]" + "\n", 0);
                write_msg_log("sRQName : [" + e.sRQName + "]" + "\n", 0);
                write_msg_log("sTrCode : [" + e.sTrCode + "]" + "\n", 0);
                write_msg_log("sMsg : [" + e.sMsg + "]" + "\n", 0);

                write_msg_log("=========매도취소주문 원장 응답정보 출력 종료==========\n", 0);
                g_flag_3 = 1; // 매도취소주문 응답 완료 설정
            }
        }

        private void axKHOpenAPI1_OnReceiveChejanData(object sender, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveChejanDataEvent e)
        {
            if(e.sGubun == "0") // sGubun 값이"0"이라면 주문내역 및 체결내역 수신
            {
                String chejan_gb = "";
                chejan_gb = jd_kw_openAPI.GetChejanData(913).Trim();
                // 주문내역인지 체결내역인지 가져옴

            if(chejan_gb == "접수")
                {
                    String user_id = null;
                    String jongmok_cd = null;
                    String jongmok_nm = null;
                    String ord_gb = null;
                    String ord_no = null;
                    String org_ord_no = null;
                    String ref_dt = null;
                    int ord_price = 0;
                    int ord_stock_cnt = 0;
                    int ord_amt = 0;
                    String ord_dtm = null;

                    user_id = g_user_id;
                    jongmok_cd = jd_kw_openAPI.GetChejanData(9001).Trim().Substring(1, 6);
                    jongmok_nm = get_jongmok_nm(jongmok_cd);
                    ord_gb = jd_kw_openAPI.GetChejanData(907).Trim();
                    ord_no = jd_kw_openAPI.GetChejanData(9203).Trim();
                    org_ord_no = jd_kw_openAPI.GetChejanData(904).Trim();
                    ord_price = int.Parse(jd_kw_openAPI.GetChejanData(901).Trim());
                    ord_stock_cnt = int.Parse(jd_kw_openAPI.GetChejanData(900).Trim());
                    ord_amt = ord_price * ord_stock_cnt;

                    DateTime CurTime;
                    String CurDt;
                    CurTime = DateTime.Now;
                    CurDt = CurTime.ToString("yyyy") + CurTime.ToString("MM") + CurTime.ToString("dd");

                    ref_dt = CurDt;

                    ord_dtm = CurDt + jd_kw_openAPI.GetChejanData(908).Trim();

                    write_msg_log("종목 코드 : [" + jongmok_cd + "]" + "\n", 0);
                    write_msg_log("종목명 : [" + jongmok_nm + "]" + "\n", 0);
                    write_msg_log("주문 구분 : [" + ord_gb + "]" + "\n", 0);
                    write_msg_log("주문 번호 : [" + ord_no + "]" + "\n", 0);
                    write_msg_log("원주문번호 : [" + org_ord_no + "]" + "\n", 0);
                    write_msg_log("주문 금액 : [" + ord_price.ToString() + "]" + "\n", 0);
                    write_msg_log("주문 주식수: [" + ord_stock_cnt.ToString() + "]" + "\n", 0);
                    write_msg_log("주문금액 : [" + ord_amt.ToString() + "]" + "\n", 0);
                    write_msg_log("주문일시 : [" + ord_dtm + "]" + "\n", 0);

                    insert_tb_ord_lst(ref_dt, jongmok_cd, jongmok_nm, ord_gb, ord_no, org_ord_no, ord_price, ord_stock_cnt, ord_amt, ord_dtm);

                    if(ord_gb == "2")
                    {
                        update_tb_accnt(ord_gb, ord_amt);
                    }

                } // if(chejan_gb == "접수") 종료
                else if(chejan_gb == "체결")
                {
                    String user_id = null;
                    String jongmok_cd = null;
                    String jongmok_nm = null;
                    String chegyul_gb = null;
                    int chegyul_no = 0;
                    int chegyul_price = 0;
                    int chegyul_cnt = 0;
                    int chegyul_amt = 0;
                    String chegyul_dtm = null;
                    String ord_no = null;
                    String org_ord_no = null;
                    string ref_dt = null;

                    user_id = g_user_id;
                    jongmok_cd = jd_kw_openAPI.GetChejanData(9001).Trim().Substring(1, 6);
                    jongmok_nm = get_jongmok_nm(jongmok_cd);
                    chegyul_gb = jd_kw_openAPI.GetChejanData(907).Trim();
                    chegyul_no = int.Parse(jd_kw_openAPI.GetChejanData(909).Trim());
                    chegyul_price = int.Parse(jd_kw_openAPI.GetChejanData(910).Trim());
                    chegyul_cnt = int.Parse(jd_kw_openAPI.GetChejanData(911).Trim());
                    chegyul_amt = chegyul_price * chegyul_cnt;
                    org_ord_no = jd_kw_openAPI.GetChejanData(904).Trim();

                    DateTime CurTime;
                    String CurDt;
                    CurTime = DateTime.Now;
                    CurDt = CurTime.ToString("yyyy") + CurTime.ToString("MM") + CurTime.ToString("dd");
                    ref_dt = CurDt;
                    chegyul_dtm = CurDt + jd_kw_openAPI.GetChejanData(908).Trim();
                    ord_no = jd_kw_openAPI.GetChejanData(9203).Trim();

                    write_msg_log("종목코드 : [" + jongmok_cd + "]" + "\n", 0);
                    write_msg_log("종목명 : [" + jongmok_nm + "]" + "\n", 0);
                    write_msg_log("체결구분 : [" + chegyul_gb + "]" + "\n", 0);
                    write_msg_log("체결번호 : [" + chegyul_no.ToString() + "]" + "\n", 0);
                    write_msg_log("체결가 : [" + chegyul_price.ToString() + "]" + "\n", 0);
                    write_msg_log("체결주식수 : [" + chegyul_cnt.ToString() + "]" + "\n", 0);
                    write_msg_log("체결금액 : [" + chegyul_amt.ToString() + "]" + "\n", 0);
                    write_msg_log("체결일시 : [" + chegyul_dtm + "]" + "\n", 0);
                    write_msg_log("주문번호 : [" + ord_no + "]" + "\n", 0);
                    write_msg_log("원주문번호 : [" + org_ord_no + "]" + "\n", 0);

                    insert_tb_chegyul_lst(ref_dt, jongmok_cd, jongmok_nm, chegyul_gb, chegyul_no, chegyul_price, chegyul_cnt, chegyul_amt,
                        chegyul_dtm, ord_no, org_ord_no); // 체결 내역 저장

                    if(chegyul_gb == "1")
                    {
                        update_tb_accnt(chegyul_gb, chegyul_amt);
                    }
                } // else if (chejan_gb == "체결") 종료
            } // if(e.sGubun == "0") 종료

            if(e.sGubun == "1")
            {
                String user_id = null;
                String jongmok_cd = null;

                int boyu_cnt = 0;
                int boyu_price = 0;
                int boyu_amt = 0;

                user_id = g_user_id;
                jongmok_cd = jd_kw_openAPI.GetChejanData(9001).Trim().Substring(1, 6);
                boyu_cnt = int.Parse(jd_kw_openAPI.GetChejanData(930).Trim());
                boyu_price = int.Parse(jd_kw_openAPI.GetChejanData(931).Trim());
                boyu_amt = int.Parse(jd_kw_openAPI.GetChejanData(932).Trim());

                String l_jongmok_nm = null;
                l_jongmok_nm = get_jongmok_nm(jongmok_cd);

                write_msg_log("종목 코드 : [ " + jongmok_cd + "]" + "\n", 0);
                write_msg_log("보유 주식수 : [ " + boyu_cnt.ToString() + "]" + "\n", 0);
                write_msg_log("보유가 : [ " + buy_price.ToString() + "]" + "\n", 0);
                write_msg_log("보유 금액 : [ " + buy_amt.ToString() + "]" + "\n", 0);

                merge_tb_accnt_info(jongmok_cd, l_jongmok_nm, boyu_cnt, boyu_price, boyu_amt); // 계좌정보(보유종목) 저장
            } // if (e.sGubun == "1") 종료

        } // 메서드 종료

        private void label1_Click(object sender, EventArgs e)
        {

        }

        

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void 로그인ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int ret = 0;
            int ret2 = 0;

            String l_accno = null;
            String l_accno_cnt = null;
            String[] l_accno_arr = null;

            ret = jd_kw_openAPI.CommConnect();

            if(ret == 0)
            {
                jd_ssr_label1.Text = "로그인 중...";

                for(; ; )
                {
                    ret2 = jd_kw_openAPI.GetConnectState();
                    if(ret2 == 1)
                    {
                        break;
                    } else
                    {
                        delay(1000);
                    }
                }

                jd_ssr_label1.Text = "로그인 완료";

                g_user_id = "";
                g_user_id = jd_kw_openAPI.GetLoginInfo("USER_ID").Trim();
                jd_txt_user_id.Text = g_user_id;

                l_accno_cnt = "";
                l_accno_cnt = jd_kw_openAPI.GetLoginInfo("ACCOUNT_CNT").Trim();
                l_accno_arr = new String[int.Parse(l_accno_cnt)];

                l_accno = "";
                l_accno = jd_kw_openAPI.GetLoginInfo("ACCNO").Trim();

                l_accno_arr = l_accno.Split(';');

                jd_cmb_acc_no.Items.Clear();
                jd_cmb_acc_no.Items.AddRange(l_accno_arr);
                jd_cmb_acc_no.SelectedIndex = 0;
                g_accnt_no = jd_cmb_acc_no.SelectedItem.ToString().Trim();
            }
        }

        private void 로그아웃ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            jd_kw_openAPI.CommTerminate();
            jd_ssr_label1.Text = "로그아웃이 완료되었습니다.";
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            g_accnt_no = jd_cmb_acc_no.SelectedItem.ToString().Trim();
            write_msg_log("사용할 증권 계좌번호는 : [" + g_accnt_no + "] 입니다. \n", 0);
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            OracleCommand cmd;
            OracleConnection conn;
            OracleDataReader reader = null;

            string sql;

            string l_jongmok_cd;
            string l_jongmok_nm;
            int l_priority;
            int l_buy_amt;
            int l_buy_price;
            int l_target_price;
            int l_cut_loss_price;
            string l_buy_trd_yn;
            string l_sell_trd_yn;
            int l_seq = 0;
            string[] l_arr = null;

            conn = null;
            conn = connect_db();



            cmd = null;

            cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;

            sql = null;

            sql = @"SELECT " +
                "JONGMOK_CD, " +
                "JONGMOK_NM, " +
                "PRIORITY, " +
                "BUY_AMT, " +
                "BUY_PRICE, " +
                "TARGET_PRICE, " +
                "CUT_LOSS_PRICE, " +
                "BUY_TRD_YN, " +
                "SELL_TRD_YN " +
                "FROM " +
                "TB_TRD_JONGMOK " +
                "WHERE USER_ID = " +
                ":g_user_id order by PRIORITY ";
            

            cmd.CommandText = sql;
            //cmd.Prepare();
            cmd.Parameters.Add("g_user_id", g_user_id);

            this.Invoke(new MethodInvoker(
                delegate ()
                {
                    jd_dgv_main.Rows.Clear();
                }));

            try
            {
                reader = cmd.ExecuteReader();
            } catch(Exception ex)
            {
                write_err_log("SELECT  TB_TRD_JONGMOK ex.Message : [" + ex.Message + "]\n", 0);
            }

            l_jongmok_cd = "";
            l_jongmok_nm = "";
            l_priority = 0;

            l_buy_amt = 0;
            l_buy_price = 0;
            l_target_price = 0;
            l_cut_loss_price = 0;
            l_buy_trd_yn = "";
            l_sell_trd_yn = "";

            while(reader.Read())
            {
                l_seq++;
                l_jongmok_cd = "";
                l_jongmok_nm = "";
                l_priority = 0;
                l_buy_amt = 0;
                l_buy_price = 0;
                l_target_price = 0;
                l_cut_loss_price = 0;
                l_buy_trd_yn = "";
                l_sell_trd_yn = "";
                l_seq = 0;

                l_jongmok_cd = reader[0].ToString().Trim();
                l_jongmok_nm = reader[1].ToString().Trim();
                l_priority = int.Parse(reader[2].ToString().Trim());
                l_buy_amt = int.Parse(reader[3].ToString().Trim());
                l_buy_price = int.Parse(reader[4].ToString().Trim());
                l_target_price = int.Parse(reader[5].ToString().Trim());
                l_cut_loss_price = int.Parse(reader[6].ToString().Trim());
                l_buy_trd_yn = reader[7].ToString().Trim();
                l_sell_trd_yn = reader[8].ToString().Trim();

                l_arr = null;
                l_arr = new String[]
                {
                    l_seq.ToString(),
                    l_jongmok_cd,
                    l_jongmok_nm,
                    l_priority.ToString(),
                    l_buy_amt.ToString(),
                    l_buy_price.ToString(),
                    l_target_price.ToString(),
                    l_cut_loss_price.ToString(),
                    l_buy_trd_yn,
                    l_sell_trd_yn
                };
                this.Invoke(new MethodInvoker(
                    delegate ()
                    {
                        jd_dgv_main.Rows.Add(l_arr);
                    }));


            }

            write_msg_log("TB_TRD_JONGMOK 테이블이 조회되었습니다.\n", 0);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OracleCommand cmd;
            OracleConnection conn;

            string sql;

            string l_jongmok_cd;
            string l_jongmok_nm;
            int l_priority;
            int l_buy_amt;
            int l_buy_price;
            int l_target_price;
            int l_cut_loss_price;
            string l_buy_trd_yn;
            string l_sell_trd_yn;
            
            foreach (DataGridViewRow Row in jd_dgv_main.Rows)
            {
                if (Convert.ToBoolean(Row.Cells[check.Name].Value) != true)
                {
                    continue;
                }
                if(Convert.ToBoolean(Row.Cells[check.Name].Value) == true)
                {
                    l_jongmok_cd = Row.Cells[1].Value.ToString();
                    l_jongmok_nm = Row.Cells[2].Value.ToString();
                    l_priority = int.Parse(Row.Cells[3].Value.ToString());
                    l_buy_amt = int.Parse(Row.Cells[4].Value.ToString());
                    l_buy_price = int.Parse(Row.Cells[5].Value.ToString());
                    l_target_price = int.Parse(Row.Cells[6].Value.ToString());
                    l_cut_loss_price = int.Parse(Row.Cells[7].Value.ToString());
                    l_buy_trd_yn = Row.Cells[8].Value.ToString();
                    l_sell_trd_yn = Row.Cells[9].Value.ToString();

                    conn = null;
                    conn = connect_db();

                    cmd = null;

                    cmd = new OracleCommand();
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;

                    sql = null;
                    //sql = @"insert into TB_TRD_JONGMOK values " +
                    //    "(" +
                    //    "'" + g_user_id + "'" + "," +
                    //    "'" + l_jongmok_cd + "'" + "," +
                    //    "'" + l_jongmok_nm + "'" + "," +
                    //        l_priority + "," +
                    //        l_buy_amt + "," +
                    //        l_buy_price + "," +
                    //        l_target_price + "," +
                    //        l_cut_loss_price + "," +
                    //        "'" + l_buy_trd_yn + "'" + "," +
                    //        "'" + l_sell_trd_yn + "'" + "," +
                    //        "'" + g_user_id + "'" + "," +
                    //        "sysdate " + "," +
                    //        "NULL" + "," +
                    //        "NULL" +
                    //    ")";

                    sql = @"insert into TB_TRD_JONGMOK values " +
                        "(" +
                        ":g_user_id, :l_jongmok_cd, :l_jongmok_nm, :l_priority, :l_buy_amt, :l_buy_price, " +
                        ":l_target_price, :l_cut_loss_price, :l_buy_trd_yn, :l_sell_trd_yn, :g_user_id, " + 
                            "sysdate " + "," +
                            "NULL" + "," +
                            "NULL" +
                        ")";

                    cmd.CommandText = sql;
                    cmd.Parameters.Add("g_user_id", g_user_id);
                    cmd.Parameters.Add("l_jongmok_cd", l_jongmok_cd);
                    cmd.Parameters.Add("l_jongmok_nm", l_jongmok_nm);
                    cmd.Parameters.Add("l_priority", l_priority);
                    cmd.Parameters.Add("l_buy_amt", l_buy_amt);
                    cmd.Parameters.Add("l_buy_price", l_buy_price);
                    cmd.Parameters.Add("l_target_price", l_target_price);
                    cmd.Parameters.Add("l_cut_loss_price", l_cut_loss_price);
                    cmd.Parameters.Add("l_buy_trd_yn", l_buy_trd_yn);
                    cmd.Parameters.Add("l_sell_trd_yn", l_sell_trd_yn);

                    try
                    {
                        
                        cmd.ExecuteNonQuery();
                       
                    } catch(Exception ex)
                    {
                        write_err_log("insert TB_TRD_JONGMOK ex.Message : [" + ex.Message + "]\n", 0);
                    }
                    write_msg_log("종목코드 : [" + l_jongmok_cd + "]" + "가 삽입되었습니다.\n", 0);
                    conn.Close();
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OracleCommand cmd;
            OracleConnection conn;

            string sql;

            string l_jongmok_cd;
            string l_jongmok_nm;
            int l_priority;
            int l_buy_amt;
            int l_buy_price;
            int l_target_price;
            int l_cut_loss_price;
            string l_buy_trd_yn;
            string l_sell_trd_yn;

            foreach (DataGridViewRow Row in jd_dgv_main.Rows)
            {
                if (Convert.ToBoolean(Row.Cells[check.Name].Value) != true)
                {
                    continue;
                }
                if (Convert.ToBoolean(Row.Cells[check.Name].Value) == true)
                {
                    l_jongmok_cd = Row.Cells[1].Value.ToString();
                    l_jongmok_nm = Row.Cells[2].Value.ToString();
                    l_priority = int.Parse(Row.Cells[3].Value.ToString());
                    l_buy_amt = int.Parse(Row.Cells[4].Value.ToString());
                    l_buy_price = int.Parse(Row.Cells[5].Value.ToString());
                    l_target_price = int.Parse(Row.Cells[6].Value.ToString());
                    l_cut_loss_price = int.Parse(Row.Cells[7].Value.ToString());
                    l_buy_trd_yn = Row.Cells[8].Value.ToString();
                    l_sell_trd_yn = Row.Cells[9].Value.ToString();

                    conn = null;
                    conn = connect_db();

                    cmd = null;

                    cmd = new OracleCommand();
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;



                    sql = null;
                    //sql = @"UPDATE TB_TRD_JONGMOK " +
                    //    "SET" +
                    //    " JONGMOK_NM = " + "'" + l_jongmok_nm + "'" + "," +
                    //    " PRIORITY = " + l_priority + "," +
                    //    " BUY_AMT = " + l_buy_amt + "," +
                    //    " BUY_PRICE = " + l_buy_price + "," +
                    //    " TARGET_PRICE = " + l_target_price + "," +
                    //    " CUT_LOSS_PRICE = " + l_cut_loss_price + "," +
                    //    " BUY_TRD_YN = " + "'" + l_buy_trd_yn + "'" + "," +
                    //    " SELL_TRD_YN = " + "'" + l_sell_trd_yn + "'" + "," +
                    //    " UPDT_ID = " + "'" + g_user_id + "'" + "," +
                    //    " UPDT_DTM = SYSDATE " +
                    //    " WHERE JONGMOK_CD = " + "'" + l_jongmok_cd + "'" +
                    //    " AND USER_ID = " + "'" + g_user_id + "'";

                    sql = @"UPDATE TB_TRD_JONGMOK SET" +
                        " jongmok_nm = :l_jongmok_nm" +
                        " priority = :l_priority," +
                        " buy_amt = :l_buy_amt," +
                        " buy_price = :l_buy_price," +
                        " target_price = :l_target_price," +
                        " cut_loss_price = :l_cut_loss_price," +
                        " buy_trd_yn = :l_buy_trd_yn," +
                        " sell_trd_yn = :l_sell_trd_yn," +
                        " updt_id = :g_user_id," +
                        " updt_dtm = SYSDATE " +
                        " WHERE jongmok_cd = :l_jongmok_cd" +
                        " AND user_id = :g_user_id";


                    cmd.CommandText = sql;
     
                    cmd.Parameters.Add("l_jongmok_nm", l_jongmok_nm);

                    cmd.Parameters.Add("l_priority", l_priority);
                    cmd.Parameters.Add("l_buy_amt", l_buy_amt);
                    cmd.Parameters.Add("l_buy_price", l_buy_price);
                    cmd.Parameters.Add("l_target_price", l_target_price);
                    cmd.Parameters.Add("l_cut_loss_price", l_cut_loss_price);
                    cmd.Parameters.Add("l_buy_trd_yn", l_buy_trd_yn);
                    cmd.Parameters.Add("l_sell_trd_yn", l_sell_trd_yn);

                    cmd.Parameters.Add("g_user_id", g_user_id);

                    cmd.Parameters.Add("l_jongmok_cd", l_jongmok_cd);
                    

                    try
                    {
                        
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        write_err_log("UPDATE TB_TRD_JONGMOK ex.Message : [" + ex.Message + "]\n", 0);

                    }
                    write_msg_log("종목코드 : [" + l_jongmok_cd + "]" + "가 수정되었습니다.\n", 0);
                    conn.Close();
                }
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            OracleCommand cmd;
            OracleConnection conn;

            string sql;

            string l_jongmok_cd = null;

            foreach (DataGridViewRow Row in jd_dgv_main.Rows)
            {
               
                if (Convert.ToBoolean(Row.Cells[check.Name].Value) != true)
                {
                    continue;
                }
                if (Convert.ToBoolean(Row.Cells[check.Name].Value) == true)
                {
                    l_jongmok_cd = Row.Cells[1].Value.ToString();

                    conn = null;
                    conn = connect_db();

                    cmd = null;
                    cmd = new OracleCommand();
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;

                    sql = null;

                    //sql = @" DELETE FROM TB_TRD_JONGMOK " +
                    //    " WHERE JONGMOK_CD = " + "'" + l_jongmok_cd + "'" +
                    //    " AND USER_ID = " + "'" + g_user_id + "'";

                    sql = @" DELETE FROM TB_TRD_JONGMOK " +
                        " WHERE JONGMOK_CD = :l_jongmok_cd" +
                        " AND USER_ID = :g_user_id";

                    cmd.CommandText = sql;
                    
                    cmd.Parameters.Add("l_jongmok_cd", l_jongmok_cd);
                    cmd.Parameters.Add("g_user_id", g_user_id);


                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        write_err_log("delete TB_TRD_JONGMOK ex.Message : [" + ex.Message + "]\n", 0);
                    }
                    write_msg_log("종목코드 : [" + l_jongmok_cd + "]" + "가 삭제되었습니다.\n", 0);
                    conn.Close();
                }
            }
        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            if(g_is_thread == 1)
            {
                write_msg_log("Auto Trading이 이미 시작되었습니다.\n", 0);
                return;
            }

            // no need
            // g_is_thread = 1;
            thread1 = new Thread(new ThreadStart(m_thread1));
            thread1.Start();
        }

        

        private void button6_Click(object sender, EventArgs e)
        {
            write_msg_log("\n 자동매매 중지 시작 \n", 0);

            try
            {
                thread1.Abort();
            } catch(Exception ex)
            {
                write_err_log("자동 매매 중지 ex.Message : " + ex.Message + "\n", 0);
            }

            this.Invoke(new MethodInvoker(() => {
                if(thread1 != null)
                {
                    thread1.Interrupt();
                    thread1 = null;
                }
            }));
            g_is_thread = 0;
            write_msg_log("\n 자동매매 중지 완료 \n", 0);
        }

        private void groupBox4_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox5_Enter(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
