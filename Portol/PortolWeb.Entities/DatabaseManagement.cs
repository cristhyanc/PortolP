using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Portol.Common.Interfaces.PortolWeb;

namespace PortolWeb.Entities
{
  public  class DatabaseManagement: IDatabaseManagement
    {
        DataContext _dataContext;
        FileInfo file;
        public DatabaseManagement(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public bool UpgradeDB(string scriptsPath)
        {
            List<Script> dbScripts, newScripts;
            string fileNumber;
            int seq;
            string[] scritps;
            try
            {
                if (! Directory.Exists(scriptsPath))
                {
                    return false;
                }

                dbScripts = _dataContext.Scripts.ToList();

                if(dbScripts==null)
                {
                    dbScripts = new List<Script>();
                }

                newScripts = new List<Script>();
                scritps = Directory.GetFiles(scriptsPath, "S*.sql");

                if (scritps?.Length>0)
                {
                    foreach (var item in scritps)
                    {
                        file = new FileInfo(item);
                      
                        var newScr = new Script();
                        newScr.ScriptName = file.Name;
                        fileNumber = Regex.Match(file.Name, @"\d+").Value;
                        if (int.TryParse(fileNumber, out seq))
                        {
                            if(dbScripts.Where(x=> x.ScriptName.Equals(file.Name)).FirstOrDefault()==null)
                            {
                                newScr.Seq = seq;
                                newScripts.Add(newScr);
                            }
                        }
                    }

                    if(newScripts?.Count>0)
                    {
                        dbScripts = new List<Script>();
                        newScripts = newScripts.OrderBy(x => x.Seq).ToList();
                        newScripts.ForEach((x) =>
                        {
                          if(  RunScript(scriptsPath + @"\" + x.ScriptName))
                            {
                                x.ScriptDate = DateTime.Now.ToUniversalTime();
                                dbScripts.Add(x);
                            }                           
                        });

                        if(dbScripts?.Count>0)
                        {
                            _dataContext.Scripts.AddRange(dbScripts);
                            _dataContext.SaveChanges();
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return true;
        }


        private bool  RunScript(string pathFile)
        {
            try
            {
                string sql = File.ReadAllText(pathFile);
                string[] commands;
                if (sql?.Length > 0)
                {
                    commands = sql.Split("GO");
                    if (commands?.Length > 0)
                    {
                        _dataContext.Database.BeginTransaction();

                        foreach (var item in commands)
                        {
                            sql = item.Trim();
                            sql = sql.Replace(Environment.NewLine, "");
                            if (sql?.Length>0)
                            {
                                _dataContext.Database.ExecuteSqlCommand(sql);
                            }                           
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _dataContext.Database.RollbackTransaction();
                return false;
            }
            _dataContext.Database.CommitTransaction();
            return true;
        }
    }
}
