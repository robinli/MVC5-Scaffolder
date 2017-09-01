using System;
using System.Data.Common;
using System.Collections;

namespace SqlHelper2 {
    public static class DbCommandExtensions {
        public static void SetParameters(this DbCommand cmd, object parameters) {
            cmd.Parameters.Clear();

            if (parameters == null)
                return;

            //if (parameters is IDictionary<string, object>) {
            //    foreach (var kvp in (IDictionary<string, object>)parameters) {
            //        AddParameter(cmd, kvp.Key, kvp.Value);
            //    }
            //}
            if (parameters is IList) {
                var listed = (IList) parameters;
                for (var i = 0; i < listed.Count; i++) {
                    AddParameter(cmd, string.Format("@{0}", i), listed[i]);
                }
            }
            else {
                var t = parameters.GetType();
                var parameterInfos = t.GetProperties();
                foreach (var pi in parameterInfos) {
                    AddParameter(cmd, pi.Name, pi.GetValue(parameters, null));
                }
            }
        }

        private static void AddParameter(DbCommand cmd, string name, object value) {
            var p = cmd.CreateParameter();
            p.ParameterName = name;
            p.Value = value ?? DBNull.Value;
            cmd.Parameters.Add(p);
        }
    }
}