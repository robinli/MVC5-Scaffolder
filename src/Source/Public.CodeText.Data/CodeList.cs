
using System.Data;
using System.Linq;
using System.Data.Linq;
using System.Collections;
using System.Collections.Generic;
namespace PublicPara.CodeText.Data
{
    public class CodeList
    {
        public CodeList() {
        }
        public CodeList(string theName, CodeListSet codeLists,IEnumerable<CodeValue> codevalues)
        {
             
            this._codeLists = null;
            this._pairs = null;
            
            this.name = theName;
            this._pairs = codevalues.ToArray();
            this._codeLists = codeLists;
          
          
        }
        public string Code2Value(string code)
        {
            string text1;
            if (this.Code2Value(code, out text1))
            {
                return text1;
            }
            return string.Empty;
        }

        public bool Code2Value(string code, out string value)
        {
        
            bool flag1 = false;
            value = string.Empty;
            if (code != null)
            {
               
                        for (int num1 = 0; num1 < this._pairs.Length; num1++)
                        {
                            if (this._pairs[num1].code == code)
                            {
                                value = this._pairs[num1].value;
                                return true;
                            }
                        }
                        return flag1;

                   
              
            }
            return flag1;
        }

         

        public CodeValue[] EnumRecords()
        {
             
                    return this._pairs;
             
        }

        public string Value2Code(string value)
        {
            string text1;
            if (this.Value2Code(value, out text1))
            {
                return text1;
            }
            return string.Empty;
        }

        public bool Value2Code(string value, out string code)
        {
            
            bool flag1 = false;
            code = string.Empty;
            if (value != null)
            {
                
                        for (int num1 = 0; num1 < this._pairs.Length; num1++)
                        {
                            if (this._pairs[num1].value == value)
                            {
                                code = this._pairs[num1].code;
                                return true;
                            }
                        }
                        return flag1;
                 
            }
            return flag1;
        }

     


    
        private CodeListSet _codeLists;
        private CodeValue[] _pairs;
      
        public string name;
    }
}
