using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Entity.SqlServer;
using Repository.Pattern.Repositories;
using Repository.Pattern.Ef6;
using System.Web.WebPages;
using WebApp.Models;


namespace WebApp.Repositories
{
    public class CodeItemQuery : QueryObject<CodeItem>
    {
        public CodeItemQuery WithAnySearch(string search)
        {
            if (!string.IsNullOrEmpty(search))
                And(x => x.Id.ToString().Contains(search) || x.Code.Contains(search) || x.Text.Contains(search) || x.Description.Contains(search) || x.IsDisabled.ToString().Contains(search));
            return this;
        }


        public CodeItemQuery Withfilter(IEnumerable<filterRule> filters)
        {
            if (filters != null)
            {
                foreach (var rule in filters)
                {


                    if (rule.field == "Id" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
                    {
                        int val = Convert.ToInt32(rule.value);

                        And(x => x.Id == val);


                    }



                    if (rule.field == "CodeType" && !string.IsNullOrEmpty(rule.value))
                    {
                        And(x => x.CodeType.Contains(rule.value));
                    }
                    if (rule.field == "Multiple" && !string.IsNullOrEmpty(rule.value))
                    {
                        var istrue = Convert.ToBoolean(rule.value);
                        And (x => x.Multiple==istrue);
                    }

                    if (rule.field == "Code" && !string.IsNullOrEmpty(rule.value))
                    {
                        And(x => x.Code.Contains(rule.value));
                    }





                    if (rule.field == "Text" && !string.IsNullOrEmpty(rule.value))
                    {
                        And(x => x.Text.Contains(rule.value));
                    }





                    if (rule.field == "Description" && !string.IsNullOrEmpty(rule.value))
                    {
                        And(x => x.Description.Contains(rule.value));
                    }






                    if (rule.field == "IsDisabled" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
                    {
                        int val = Convert.ToInt32(rule.value);

                        And(x => x.IsDisabled == val);


                    }











                }
            }
            return this;
        }



        public CodeItemQuery ByBaseCodeIdWithfilter(int basecodeid, IEnumerable<filterRule> filters)
        {


            if (filters != null)
            {
                foreach (var rule in filters)
                {



                    if (rule.field == "Id" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
                    {
                        int val = Convert.ToInt32(rule.value);
                        switch (rule.op)
                        {
                            case "equal":
                                And(x => x.Id == val);
                                break;
                            case "notequal":
                                And(x => x.Id != val);
                                break;
                            case "less":
                                And(x => x.Id < val);
                                break;
                            case "lessorequal":
                                And(x => x.Id <= val);
                                break;
                            case "greater":
                                And(x => x.Id > val);
                                break;
                            case "greaterorequal":
                                And(x => x.Id >= val);
                                break;
                            default:
                                And(x => x.Id == val);
                                break;
                        }
                    }




                    if (rule.field == "Code" && !string.IsNullOrEmpty(rule.value))
                    {
                        And(x => x.Code.Contains(rule.value));
                    }





                    if (rule.field == "Text" && !string.IsNullOrEmpty(rule.value))
                    {
                        And(x => x.Text.Contains(rule.value));
                    }





                    if (rule.field == "Description" && !string.IsNullOrEmpty(rule.value))
                    {
                        And(x => x.Description.Contains(rule.value));
                    }






                    if (rule.field == "IsDisabled" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
                    {
                        int val = Convert.ToInt32(rule.value);
                        switch (rule.op)
                        {
                            case "equal":
                                And(x => x.IsDisabled == val);
                                break;
                            case "notequal":
                                And(x => x.IsDisabled != val);
                                break;
                            case "less":
                                And(x => x.IsDisabled < val);
                                break;
                            case "lessorequal":
                                And(x => x.IsDisabled <= val);
                                break;
                            case "greater":
                                And(x => x.IsDisabled > val);
                                break;
                            case "greaterorequal":
                                And(x => x.IsDisabled >= val);
                                break;
                            default:
                                And(x => x.IsDisabled == val);
                                break;
                        }
                    }










                }
            }
            return this;
        }

    }
}



