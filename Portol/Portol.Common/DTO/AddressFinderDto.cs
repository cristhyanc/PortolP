using System;
using System.Collections.Generic;
using System.Text;

namespace Portol.Common.DTO
{
    public class AddressFinderDto
    {
        public List<AddressFinderResultDto> completions { get; set; }
        public string paid { get; set; }
        public string success { get; set; }
    }

    public class AddressFinderResultDto
    {
        public string id { get; set; }
        public string full_address { get; set; }
        public string canonical_address_id { get; set; }
    }

    public class AddressFinderDetail
    {
        public string locality_name { get; set; }
        public string state_territory { get; set; }
        public string address_line_1 { get; set; }
        public string address_line_2 { get; set; }
        public string postcode { get; set; }
    }
}
