using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.Linq;
using System.Linq;

namespace PublicPara.CodeText.Data
{
    public class CodeListSet
    {
        static CodeListSet()
        {
            CodeListSet.CLS = new CodeListSet();

            

           
            CodeListSet.CLS.Init();
        }

        public CodeListSet()
        {
            
            this.codeLists = new Hashtable();

        }

        

        //public string Code2Value(CodeLists codeList, string code)
        //{
        //    CodeList list1 = this[codeList];
        //    if (list1 == null)
        //    {
        //        return string.Empty;
        //    }
        //    return list1.Code2Value(code);
        //}

        public string Code2Value(string codeList, string code)
        {
            CodeList list1 = this[codeList];
            if (list1 == null)
            {
                return string.Empty;
            }
            return list1.Code2Value(code);
        }

        //public bool Code2Value(CodeLists codeList, string code, out string value)
        //{
        //    CodeList list1 = this[codeList];
        //    if (list1 == null)
        //    {
        //        value = string.Empty;
        //        return false;
        //    }
        //    return list1.Code2Value(code, out value);
        //}

        public bool Code2Value(string codeList, string code, out string value)
        {
            CodeList list1 = this[codeList];
            if (list1 == null)
            {
                value = string.Empty;
                return false;
            }
            return list1.Code2Value(code, out value);
        }

        public bool Init()
        {

           
                string text = string.Empty;
            try
            {
                
                CodeList codeList;
                var list = GetData();
                var codevalues = new List<CodeValue>();
                foreach (var item in list.Select(x => new { CodeType = x.CodeType, Description = x.Description }).Distinct()
           )        {
                    text = item.CodeType;

                    foreach(var data in list.Where(x=>x.CodeType==item.CodeType).OrderBy(x=>x.Code))
                    {
                        codevalues.Add(new CodeValue() { code = data.Code, value = data.Text });

                    }
                    codeList = new CodeList(text, this, codevalues);
                    this.codeLists.Add(text, codeList);

                }



                }
                catch (Exception e)
                {
                     
                    return false;
                }
         
                 
             
            return true;
        }
        private IEnumerable<CodeItem> GetData()
        {

            using (SqlConnection db = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                db.Open();
                var cmd = db.CreateCommand();
                string sql = "select codetype,code,text,Description from Codeitems where isDisabled = 0 order by codetype, code";

                cmd.CommandText = sql;
                var reader = cmd.ExecuteReader();
                var list = new List<CodeItem>();
                while (reader.Read())
                {

                    var codetype = reader.GetString(0);
                    var code = reader.GetString(1);
                    var text = reader.GetString(2);
                    var description = reader.GetString(3);
                    list.Add(new CodeItem() { CodeType = codetype, Code = code, Text = text, Description = description });
                }
                db.Close();
                return list;
            }

        }
        //public string Value2Code(CodeLists codeList, string value)
        //{
        //    CodeList list1 = this[codeList];
        //    if (list1 == null)
        //    {
        //        return string.Empty;
        //    }
        //    return list1.Value2Code(value);
        //}

        public string Value2Code(string codeList, string value)
        {
            CodeList list1 = this[codeList];
            if (list1 == null)
            {
                return string.Empty;
            }
            return list1.Value2Code(value);
        }

        //public bool Value2Code(CodeLists codeList, string value, out string code)
        //{
        //    CodeList list1 = this[codeList];
        //    if (list1 == null)
        //    {
        //        code = string.Empty;
        //        return false;
        //    }
        //    return list1.Value2Code(value, out code);
        //}

        public bool Value2Code(string codeList, string value, out string code)
        {
            CodeList list1 = this[codeList];
            if (list1 == null)
            {
                code = string.Empty;
                return false;
            }
            return list1.Value2Code(value, out code);
        }


        //public CodeList this[CodeLists index]
        //{
        //    get
        //    {
        //        string listName = index.ToString();
        //        if (!string.IsNullOrEmpty(listName))
        //        {
        //            return (CodeList)this.codeLists[listName];
        //        }
        //        int num1 = ((int)index) - 1;
        //        if ((num1 >= 0) && (num1 < CodeListSet._codeListEntries.Length))
        //        {
        //            return (CodeList)this.codeLists[CodeListSet._codeListEntries[num1].name];
        //        }
        //        return null;
        //    }
        //}

        public CodeList this[string listName]
        {
            get
            {
                if (listName == null)
                {
                    return null;
                }
                return (CodeList)this.codeLists[listName];
            }
        }


         
        public static CodeListSet CLS;
        public Hashtable codeLists;
 
        


         
        private class CodeListEntry
        {
            public CodeLists index;
            public string name;
            public CodeListEntry(CodeLists theIndex, string theName)
            {
                this.index = theIndex;
                this.name = theName;
            }

        }
    }
}