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

        /// <summary>
        /// Update the Database, execute any sql added in the sql folder and create the first table of the DB
        /// </summary>
        /// <param name="scriptsPath"></param>
        /// <returns></returns>
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
                
                CreateScriptTable();
              
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
                Serilog.Log.Error(ex, "UpgradeDB");
                throw ex;
            }

            return true;
        }

        private void CreateScriptTable()
        {
            string sql= " if not exists (select * from sysobjects where name='tblScript' and xtype='U') Begin " +
                        "CREATE TABLE [dbo].[tblScript]( " +
                        "    [ScriptID][uniqueidentifier] NOT NULL, " +
                        "    [ScriptName] [nvarchar] (100) NOT NULL, " +
                        "    [Seq] [int] NOT NULL, " +
                        "    [ScriptDate] [date] NOT NULL, " +
                        "    CONSTRAINT[PK_tblScripts] PRIMARY KEY CLUSTERED( " +
                        "    [ScriptID] ASC  " +
                        "    )WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON[PRIMARY] " +
                        "    ) ON[PRIMARY] End ";

            _dataContext.Database.ExecuteSqlCommand(sql);

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
