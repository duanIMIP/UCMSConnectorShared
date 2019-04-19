using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMIP.UniversalScan.Data
{
    public class Branch
    {
        public string Name { get; set; }
        public bool CacheBatches { get; set; }
        public string SharedFolder { get; set; }
        public List<Account> Users { get; set; }

        public Branch()
        {
            this.Name = "";
            this.SharedFolder = "";
            this.Users = new List<Account>();
        }

        public string GetDisplayText()
        {
            string strUsers = "";
            foreach (var user in this.Users)
            {
                strUsers += user.Name + ";";
            }

            return strUsers;
        }

        public List<string> GetGroupNames()
        {
            List<string> lst = new List<string>();
            foreach (var user in this.Users)
            {
                if (user.IsGroup())
                    lst.Add(user.Name);
            }

            return lst;
        }
    }

    public class Account
    {
        string DisplayFormat = "{0} ({1})";
        public const string Everyone = "Everyone";
        public string Name { get; set; }

        // NTAccount format 'DomainName/UserName'
        public string UPN { get; set; }
        public string Class { get; set; }

        public Account()
        {
            this.Name = "";
            this.UPN = "";
            this.Class = "";
        }

        public bool IsGroup()
        {
            return this.Class.ToLower() == "group";
        }

        public bool IsUser()
        {
            return this.Class.ToLower() == "user";
        }

        public string GetDisplayInfo()
        {
            if (string.IsNullOrEmpty(this.UPN))
                return this.Name;
            else
                return string.Format(DisplayFormat, Name, UPN);
        }

        public string GetAccountWithoutDomain()
        {
            if (UPN.IndexOf('\\') > 0)
            {
                return UPN.Substring(UPN.IndexOf('\\') + 1);
            }

            return "";
        }

        public string GetDomain()
        {
            if (UPN.IndexOf('\\') > 0)
            {
                return UPN.Substring(0, UPN.IndexOf('\\'));
            }

            return "";
        }
    }

    public class ReportParam
    {
        public string ParamName { get;set;}
        public string ParamLabel { get; set; }
        public string ParamType { get; set; }//TEXT, DATE
    }
    public class Report
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Account> Users { get; set; }

        public List<ReportParam> Params {get;set;}

        public Report()
        {
            this.Name = "";
            this.Description = "";
            this.Users = new List<Account>();
            this.Params = new List<ReportParam>();
        }

        public string GetDisplayText()
        {
            string strUsers = "";
            foreach (var user in this.Users)
            {
                strUsers += user.Name + ";";
            }

            return strUsers;
        }

        public List<string> GetGroupNames()
        {
            List<string> lst = new List<string>();
            foreach (var user in this.Users)
            {
                if (user.IsGroup())
                    lst.Add(user.Name);
            }

            return lst;
        }
    }

    //public class GPProcess
    //{
    //    public string Name { get; set; }
    //    public List<string> ProcessSteps { get; set; }

    //    public GPProcess()
    //    {
    //        this.Name = "";
    //        this.ProcessSteps = new List<string>();
    //    }
    //}

    //public class GPClient
    //{
    //    public string Name { get; set; }
    //    public List<GPProcess> Processes { get; set; }

    //    public GPClient()
    //    {
    //        this.Name = "";
    //        this.Processes = new List<GPProcess>();
    //    }
    //}

    [Serializable]
    public class xBoundItem
    {
        public string Name { get; set; }
        public List<xBoundItem> Children { get; set; }

        public xBoundItem(string name)
        {
            Name = name;
            Children = new List<xBoundItem>();
        }

        public xBoundItem()
        {
            Children = new List<xBoundItem>();
        }

        public void AddItem(xBoundItem item)
        {
            if (Children == null)
                Children = new List<xBoundItem>();

            foreach (var child in Children)
            {
                if (child.Name == item.Name)
                {
                    child.AddRange(item.Children);
                    return;
                }
            }

            Children.Add(item);
        }

        public void AddRange(List<xBoundItem> items)
        {
            foreach (var item in items)
            {
                AddItem(item);
            }
        }

        public void Remove(xBoundItem item)
        {
            foreach (var child in Children)
            {
                if (child.Name == item.Name)
                {
                    Children.Remove(child);
                    break;
                }
            }
        }

        public bool Contain(string name)
        {
            if (Children == null || Children.Count <= 0)
                return false;

            foreach (var child in Children)
            {
                if (child.Name == name)
                    return true;
            }

            return false;
        }

        public xBoundItem GetChildByName(string name)
        {
            if (Children == null || Children.Count <= 0)
                return null;

            foreach (var child in Children)
            {
                if (child.Name == name)
                    return child;
            }

            return null;
        }
    }

    [Serializable]
    public class NonADAccount
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public string Class { get; set; }
        public List<string> Groups { get; set; }
        public List<Branch> Branches { get; set; }
        public List<xBoundItem> Clients { get; set; }

        public NonADAccount()
        {
            this.Name = "";
            this.Password = "";
            this.Class = "";
            this.Groups = new List<string>();
            this.Branches = new List<Branch>();
            this.Clients = new List<xBoundItem>();
        }

        public bool IsGroup()
        {
            return this.Class.ToLower() == "group";
        }

        public bool IsUser()
        {
            return this.Class.ToLower() == "user";
        }
    }
}
