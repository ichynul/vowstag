﻿//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Data.EntityClient;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;

[assembly: EdmSchemaAttribute()]
namespace model
{
    #region 上下文
    
    /// <summary>
    /// 没有元数据文档可用。
    /// </summary>
    public partial class Entities : ObjectContext
    {
        #region 构造函数
    
        /// <summary>
        /// 请使用应用程序配置文件的“Entities”部分中的连接字符串初始化新 Entities 对象。
        /// </summary>
        public Entities() : base("name=Entities", "Entities")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        /// <summary>
        /// 初始化新的 Entities 对象。
        /// </summary>
        public Entities(string connectionString) : base(connectionString, "Entities")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        /// <summary>
        /// 初始化新的 Entities 对象。
        /// </summary>
        public Entities(EntityConnection connection) : base(connection, "Entities")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        #endregion
    
        #region 分部方法
    
        partial void OnContextCreated();
    
        #endregion
    
        #region ObjectSet 属性
    
        /// <summary>
        /// 没有元数据文档可用。
        /// </summary>
        public ObjectSet<Category> Category
        {
            get
            {
                if ((_Category == null))
                {
                    _Category = base.CreateObjectSet<Category>("Category");
                }
                return _Category;
            }
        }
        private ObjectSet<Category> _Category;
    
        /// <summary>
        /// 没有元数据文档可用。
        /// </summary>
        public ObjectSet<User> User
        {
            get
            {
                if ((_User == null))
                {
                    _User = base.CreateObjectSet<User>("User");
                }
                return _User;
            }
        }
        private ObjectSet<User> _User;
    
        /// <summary>
        /// 没有元数据文档可用。
        /// </summary>
        public ObjectSet<Article> Article
        {
            get
            {
                if ((_Article == null))
                {
                    _Article = base.CreateObjectSet<Article>("Article");
                }
                return _Article;
            }
        }
        private ObjectSet<Article> _Article;
    
        /// <summary>
        /// 没有元数据文档可用。
        /// </summary>
        public ObjectSet<Admin> Admin
        {
            get
            {
                if ((_Admin == null))
                {
                    _Admin = base.CreateObjectSet<Admin>("Admin");
                }
                return _Admin;
            }
        }
        private ObjectSet<Admin> _Admin;

        #endregion

        #region AddTo 方法
    
        /// <summary>
        /// 用于向 Category EntitySet 添加新对象的方法，已弃用。请考虑改用关联的 ObjectSet&lt;T&gt; 属性的 .Add 方法。
        /// </summary>
        public void AddToCategory(Category category)
        {
            base.AddObject("Category", category);
        }
    
        /// <summary>
        /// 用于向 User EntitySet 添加新对象的方法，已弃用。请考虑改用关联的 ObjectSet&lt;T&gt; 属性的 .Add 方法。
        /// </summary>
        public void AddToUser(User user)
        {
            base.AddObject("User", user);
        }
    
        /// <summary>
        /// 用于向 Article EntitySet 添加新对象的方法，已弃用。请考虑改用关联的 ObjectSet&lt;T&gt; 属性的 .Add 方法。
        /// </summary>
        public void AddToArticle(Article article)
        {
            base.AddObject("Article", article);
        }
    
        /// <summary>
        /// 用于向 Admin EntitySet 添加新对象的方法，已弃用。请考虑改用关联的 ObjectSet&lt;T&gt; 属性的 .Add 方法。
        /// </summary>
        public void AddToAdmin(Admin admin)
        {
            base.AddObject("Admin", admin);
        }

        #endregion

    }

    #endregion

    #region 实体
    
    /// <summary>
    /// 没有元数据文档可用。
    /// </summary>
    [EdmEntityTypeAttribute(NamespaceName="Model", Name="Admin")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    public partial class Admin : EntityObject
    {
        #region 工厂方法
    
        /// <summary>
        /// 创建新的 Admin 对象。
        /// </summary>
        /// <param name="id">ID 属性的初始值。</param>
        public static Admin CreateAdmin(global::System.Int64 id)
        {
            Admin admin = new Admin();
            admin.ID = id;
            return admin;
        }

        #endregion

        #region 基元属性
    
        /// <summary>
        /// 没有元数据文档可用。
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=true, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int64 ID
        {
            get
            {
                return _ID;
            }
            set
            {
                if (_ID != value)
                {
                    OnIDChanging(value);
                    ReportPropertyChanging("ID");
                    _ID = StructuralObject.SetValidValue(value);
                    ReportPropertyChanged("ID");
                    OnIDChanged();
                }
            }
        }
        private global::System.Int64 _ID;
        partial void OnIDChanging(global::System.Int64 value);
        partial void OnIDChanged();
    
        /// <summary>
        /// 没有元数据文档可用。
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String Username
        {
            get
            {
                return _Username;
            }
            set
            {
                OnUsernameChanging(value);
                ReportPropertyChanging("Username");
                _Username = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("Username");
                OnUsernameChanged();
            }
        }
        private global::System.String _Username;
        partial void OnUsernameChanging(global::System.String value);
        partial void OnUsernameChanged();
    
        /// <summary>
        /// 没有元数据文档可用。
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String Password
        {
            get
            {
                return _Password;
            }
            set
            {
                OnPasswordChanging(value);
                ReportPropertyChanging("Password");
                _Password = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("Password");
                OnPasswordChanged();
            }
        }
        private global::System.String _Password;
        partial void OnPasswordChanging(global::System.String value);
        partial void OnPasswordChanged();
    
        /// <summary>
        /// 没有元数据文档可用。
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String Powers
        {
            get
            {
                return _Powers;
            }
            set
            {
                OnPowersChanging(value);
                ReportPropertyChanging("Powers");
                _Powers = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("Powers");
                OnPowersChanged();
            }
        }
        private global::System.String _Powers;
        partial void OnPowersChanging(global::System.String value);
        partial void OnPowersChanged();

        #endregion

    
    }
    
    /// <summary>
    /// 没有元数据文档可用。
    /// </summary>
    [EdmEntityTypeAttribute(NamespaceName="Model", Name="Article")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    public partial class Article : EntityObject
    {
        #region 工厂方法
    
        /// <summary>
        /// 创建新的 Article 对象。
        /// </summary>
        /// <param name="id">ID 属性的初始值。</param>
        public static Article CreateArticle(global::System.Int64 id)
        {
            Article article = new Article();
            article.ID = id;
            return article;
        }

        #endregion

        #region 基元属性
    
        /// <summary>
        /// 没有元数据文档可用。
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=true, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int64 ID
        {
            get
            {
                return _ID;
            }
            set
            {
                if (_ID != value)
                {
                    OnIDChanging(value);
                    ReportPropertyChanging("ID");
                    _ID = StructuralObject.SetValidValue(value);
                    ReportPropertyChanged("ID");
                    OnIDChanged();
                }
            }
        }
        private global::System.Int64 _ID;
        partial void OnIDChanging(global::System.Int64 value);
        partial void OnIDChanged();
    
        /// <summary>
        /// 没有元数据文档可用。
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<global::System.Int32> Categ
        {
            get
            {
                return _Categ;
            }
            set
            {
                OnCategChanging(value);
                ReportPropertyChanging("Categ");
                _Categ = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("Categ");
                OnCategChanged();
            }
        }
        private Nullable<global::System.Int32> _Categ;
        partial void OnCategChanging(Nullable<global::System.Int32> value);
        partial void OnCategChanged();
    
        /// <summary>
        /// 没有元数据文档可用。
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<global::System.Int64> UID
        {
            get
            {
                return _UID;
            }
            set
            {
                OnUIDChanging(value);
                ReportPropertyChanging("UID");
                _UID = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("UID");
                OnUIDChanged();
            }
        }
        private Nullable<global::System.Int64> _UID;
        partial void OnUIDChanging(Nullable<global::System.Int64> value);
        partial void OnUIDChanged();
    
        /// <summary>
        /// 没有元数据文档可用。
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String Title
        {
            get
            {
                return _Title;
            }
            set
            {
                OnTitleChanging(value);
                ReportPropertyChanging("Title");
                _Title = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("Title");
                OnTitleChanged();
            }
        }
        private global::System.String _Title;
        partial void OnTitleChanging(global::System.String value);
        partial void OnTitleChanged();
    
        /// <summary>
        /// 没有元数据文档可用。
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String Desc
        {
            get
            {
                return _Desc;
            }
            set
            {
                OnDescChanging(value);
                ReportPropertyChanging("Desc");
                _Desc = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("Desc");
                OnDescChanged();
            }
        }
        private global::System.String _Desc;
        partial void OnDescChanging(global::System.String value);
        partial void OnDescChanged();
    
        /// <summary>
        /// 没有元数据文档可用。
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String Author
        {
            get
            {
                return _Author;
            }
            set
            {
                OnAuthorChanging(value);
                ReportPropertyChanging("Author");
                _Author = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("Author");
                OnAuthorChanged();
            }
        }
        private global::System.String _Author;
        partial void OnAuthorChanging(global::System.String value);
        partial void OnAuthorChanged();
    
        /// <summary>
        /// 没有元数据文档可用。
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String Img
        {
            get
            {
                return _Img;
            }
            set
            {
                OnImgChanging(value);
                ReportPropertyChanging("Img");
                _Img = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("Img");
                OnImgChanged();
            }
        }
        private global::System.String _Img;
        partial void OnImgChanging(global::System.String value);
        partial void OnImgChanged();
    
        /// <summary>
        /// 没有元数据文档可用。
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String Content
        {
            get
            {
                return _Content;
            }
            set
            {
                OnContentChanging(value);
                ReportPropertyChanging("Content");
                _Content = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("Content");
                OnContentChanged();
            }
        }
        private global::System.String _Content;
        partial void OnContentChanging(global::System.String value);
        partial void OnContentChanged();
    
        /// <summary>
        /// 没有元数据文档可用。
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<global::System.Int32> View
        {
            get
            {
                return _View;
            }
            set
            {
                OnViewChanging(value);
                ReportPropertyChanging("View");
                _View = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("View");
                OnViewChanged();
            }
        }
        private Nullable<global::System.Int32> _View;
        partial void OnViewChanging(Nullable<global::System.Int32> value);
        partial void OnViewChanged();
    
        /// <summary>
        /// 没有元数据文档可用。
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<global::System.Boolean> IsLock
        {
            get
            {
                return _IsLock;
            }
            set
            {
                OnIsLockChanging(value);
                ReportPropertyChanging("IsLock");
                _IsLock = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("IsLock");
                OnIsLockChanged();
            }
        }
        private Nullable<global::System.Boolean> _IsLock;
        partial void OnIsLockChanging(Nullable<global::System.Boolean> value);
        partial void OnIsLockChanged();
    
        /// <summary>
        /// 没有元数据文档可用。
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<global::System.Boolean> IsTop
        {
            get
            {
                return _IsTop;
            }
            set
            {
                OnIsTopChanging(value);
                ReportPropertyChanging("IsTop");
                _IsTop = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("IsTop");
                OnIsTopChanged();
            }
        }
        private Nullable<global::System.Boolean> _IsTop;
        partial void OnIsTopChanging(Nullable<global::System.Boolean> value);
        partial void OnIsTopChanged();
    
        /// <summary>
        /// 没有元数据文档可用。
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<global::System.DateTime> Time
        {
            get
            {
                return _Time;
            }
            set
            {
                OnTimeChanging(value);
                ReportPropertyChanging("Time");
                _Time = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("Time");
                OnTimeChanged();
            }
        }
        private Nullable<global::System.DateTime> _Time;
        partial void OnTimeChanging(Nullable<global::System.DateTime> value);
        partial void OnTimeChanged();
    
        /// <summary>
        /// 没有元数据文档可用。
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<global::System.Int32> ding
        {
            get
            {
                return _ding;
            }
            set
            {
                OndingChanging(value);
                ReportPropertyChanging("ding");
                _ding = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("ding");
                OndingChanged();
            }
        }
        private Nullable<global::System.Int32> _ding;
        partial void OndingChanging(Nullable<global::System.Int32> value);
        partial void OndingChanged();

        #endregion

    
    }
    
    /// <summary>
    /// 没有元数据文档可用。
    /// </summary>
    [EdmEntityTypeAttribute(NamespaceName="Model", Name="Category")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    public partial class Category : EntityObject
    {
        #region 工厂方法
    
        /// <summary>
        /// 创建新的 Category 对象。
        /// </summary>
        /// <param name="id">ID 属性的初始值。</param>
        public static Category CreateCategory(global::System.Int64 id)
        {
            Category category = new Category();
            category.ID = id;
            return category;
        }

        #endregion

        #region 基元属性
    
        /// <summary>
        /// 没有元数据文档可用。
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=true, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int64 ID
        {
            get
            {
                return _ID;
            }
            set
            {
                if (_ID != value)
                {
                    OnIDChanging(value);
                    ReportPropertyChanging("ID");
                    _ID = StructuralObject.SetValidValue(value);
                    ReportPropertyChanged("ID");
                    OnIDChanged();
                }
            }
        }
        private global::System.Int64 _ID;
        partial void OnIDChanging(global::System.Int64 value);
        partial void OnIDChanged();
    
        /// <summary>
        /// 没有元数据文档可用。
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String Name
        {
            get
            {
                return _Name;
            }
            set
            {
                OnNameChanging(value);
                ReportPropertyChanging("Name");
                _Name = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("Name");
                OnNameChanged();
            }
        }
        private global::System.String _Name;
        partial void OnNameChanging(global::System.String value);
        partial void OnNameChanged();
    
        /// <summary>
        /// 没有元数据文档可用。
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String Desc
        {
            get
            {
                return _Desc;
            }
            set
            {
                OnDescChanging(value);
                ReportPropertyChanging("Desc");
                _Desc = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("Desc");
                OnDescChanged();
            }
        }
        private global::System.String _Desc;
        partial void OnDescChanging(global::System.String value);
        partial void OnDescChanged();
    
        /// <summary>
        /// 没有元数据文档可用。
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String Logo
        {
            get
            {
                return _Logo;
            }
            set
            {
                OnLogoChanging(value);
                ReportPropertyChanging("Logo");
                _Logo = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("Logo");
                OnLogoChanged();
            }
        }
        private global::System.String _Logo;
        partial void OnLogoChanging(global::System.String value);
        partial void OnLogoChanged();

        #endregion

    
    }
    
    /// <summary>
    /// 没有元数据文档可用。
    /// </summary>
    [EdmEntityTypeAttribute(NamespaceName="Model", Name="User")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    public partial class User : EntityObject
    {
        #region 工厂方法
    
        /// <summary>
        /// 创建新的 User 对象。
        /// </summary>
        /// <param name="id">ID 属性的初始值。</param>
        public static User CreateUser(global::System.Int64 id)
        {
            User user = new User();
            user.ID = id;
            return user;
        }

        #endregion

        #region 基元属性
    
        /// <summary>
        /// 没有元数据文档可用。
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=true, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int64 ID
        {
            get
            {
                return _ID;
            }
            set
            {
                if (_ID != value)
                {
                    OnIDChanging(value);
                    ReportPropertyChanging("ID");
                    _ID = StructuralObject.SetValidValue(value);
                    ReportPropertyChanged("ID");
                    OnIDChanged();
                }
            }
        }
        private global::System.Int64 _ID;
        partial void OnIDChanging(global::System.Int64 value);
        partial void OnIDChanged();
    
        /// <summary>
        /// 没有元数据文档可用。
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String Name
        {
            get
            {
                return _Name;
            }
            set
            {
                OnNameChanging(value);
                ReportPropertyChanging("Name");
                _Name = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("Name");
                OnNameChanged();
            }
        }
        private global::System.String _Name;
        partial void OnNameChanging(global::System.String value);
        partial void OnNameChanged();
    
        /// <summary>
        /// 没有元数据文档可用。
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String Gender
        {
            get
            {
                return _Gender;
            }
            set
            {
                OnGenderChanging(value);
                ReportPropertyChanging("Gender");
                _Gender = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("Gender");
                OnGenderChanged();
            }
        }
        private global::System.String _Gender;
        partial void OnGenderChanging(global::System.String value);
        partial void OnGenderChanged();

        #endregion

    
    }

    #endregion

    
}
