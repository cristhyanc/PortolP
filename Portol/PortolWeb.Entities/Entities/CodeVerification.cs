using Portol.Common.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PortolWeb.Entities
{
    [Table("tblCodeVerification")]
   public class CodeVerification
    {
        [Key]
        public Guid CodeID { get; set; }
        public Int32 CodeNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string CountryCode { get; set; }        
        public DateTime Created { get; set; }



        public static CodeVerificationDto ORM(CodeVerification code)
        {
            if (code == null)
            {
                return null;
            }

            CodeVerificationDto result = new CodeVerificationDto();
            result.CodeID = code.CodeID;
            result.CodeNumber = code.CodeNumber;
            result.PhoneNumber = code.PhoneNumber;
            result.CountryCode = code.CountryCode;
            result.Created = code.Created;           
            return result;
        }

        public static CodeVerification ORM(CodeVerificationDto code)
        {
            if (code == null)
            {
                return null;
            }

            CodeVerification result = new CodeVerification();
            result.CodeID = code.CodeID;
            result.CodeNumber = code.CodeNumber;
            result.PhoneNumber = code.PhoneNumber;
            result.CountryCode = code.CountryCode;
            result.Created = code.Created;
            return result;
        }


    }



}
