using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using model;

public partial class _Tables : Page
{
    private Entities db = new Entities();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindTables(null);
        }
    }

    private void BindTables(string def)
    {
        tables.Items.Clear();
        List<string> tableNames = getTables();
        tables.DataSource = tableNames;
        tables.DataBind();
        if (tables.Items.Count > 0)
        {
            if (def != null)
            {
                tables.SelectedValue = def;
            }
            else
            {
                tables.Items[0].Selected = true;
            }
            BindTableFields(tableNames);
        }
    }

    private List<string> getTables()
    {
        PropertyInfo[] ps = db.GetType().GetProperties();
        List<string> tablenames = new List<string>();
        string type;
        foreach (var p in ps)
        {
            if (p.PropertyType.IsGenericType)
            {
                type = p.PropertyType.GetGenericTypeDefinition().Name;
                if (type.Contains("ObjectSet") || type.Contains("DbSet"))
                {
                    tablenames.Add(p.Name);
                }
            }
        }

        return tablenames;
    }
    private void BindTableFields(List<string> tablenames)
    {
        if (tables.SelectedValue == "")
        {
            return;
        }
        object model = getModObj(tables.SelectedValue);

        if (model != null)
        {
            PropertyInfo[] ps = model.GetType().GetProperties();
            string type = "";
            string name = "";
            bool nullable = false;
            List<TableField> fields = new List<TableField>();
            List<ListItem[]> ctrls = new List<ListItem[]>();
            bool[] NotEmptyArr = new bool[ps.Length];
            string[] types = new string[ps.Length];
            for (int i = 0; i < ps.Length; i += 1)
            {
                name = ps[i].Name;
                type = getTypeName(model, name, out nullable);
                types[i] = type;
                NotEmptyArr[i] = type != "String" && !nullable;

                if (type == "EntityState" || type == "EntityKey")
                {
                    continue;
                }
                fields.Add(new TableField { Name = name, Type = type, NullAble = nullable ? "<b style='color:blue; '>是</b>" : "<b style='color:green; '>否</b>" });
            }
            taableFields.DataSource = fields;
            taableFields.DataBind();
        }
    }

    protected void refreshTables(object sender, EventArgs e)
    {
        BindTables(tables.SelectedValue);
    }

    protected void ReBindTableFields(object sender, EventArgs e)
    {
        BindTableFields(getTables());
    }

    #region tools
    public string getTypeName(object obj, string name, out bool nullable)
    {
        nullable = false;
        PropertyInfo pi = obj.GetType().GetProperties().FirstOrDefault(a => a.Name.ToLower() == name.ToLower());
        Type columnType = pi.PropertyType;
        if (pi.PropertyType.IsGenericType && pi.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
        {
            columnType = columnType.GetGenericArguments()[0];
            nullable = true;
        }
        return columnType.Name;
    }

    public object getModObj(string tableName)
    {
        Type ts = db.GetType();
        PropertyInfo ps = ts.GetProperties().FirstOrDefault(a => a.Name.ToLower() == tableName.ToLower());
        if (ps == null)
        {
            return null; ;
        }
        var v = ps.GetValue(db, null);
        object model = v.GetType().GetMethods().FirstOrDefault(a => a.Name == "CreateObject" || a.Name == "Create").Invoke(v, null);
        return model;
    }
    #endregion
}
sealed class TableField
{
    public string Name { get; set; }
    public string Type { get; set; }
    public string NullAble { get; set; }
}
