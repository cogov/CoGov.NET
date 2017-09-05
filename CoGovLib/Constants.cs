using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoGov.Models
{

    public class LEDGER_ACTION_TYPE
    {
        public static Guid CreateCollective = new Guid("D92AEBD6-8908-4223-9B1B-203C331B5EA0");
        public static Guid General = new Guid("7AE402B5-5D10-4D38-9834-2CC1EA399E47");
        public static Guid SetCollectiveName = new Guid("CC9B134E-8AA7-4845-87C5-D04BEBC31055");
        public static Guid SetCollectiveDetails = new Guid("{F2BE2A36-F499-45F8-822B-CB8681D6C533}");
        public static Guid SetCollectiveDescription = new Guid("{13B790BC-76D3-4D09-A128-872BE85F4E3D}");
        public static Guid CreateShareType = new Guid("D3A89E70-B31E-47ED-BD79-1A9FB6D4095A");
        public static Guid SetShareQuantity = new Guid("E3B9EA00-2918-455F-9CBB-BC7253C59280");
        public static Guid AddMember = new Guid("D743A587-BC90-48F2-A2D6-49BCF3995B59");
        public static Guid SetPermission = new Guid("96B0194F-E063-44B3-85EE-C56651A6DC18");
        public static Guid SetLedgerAccess = new Guid("61D6F27F-F6FB-43FA-82EF-12C3F2B54404");
        public static Guid GiveCollectiveAccessToLedger = new Guid("BD2D286C-16E3-4EF9-9139-38D9B90FCF49");
        public static Guid IncreaseVoteClout = new Guid("1BBF5F47-4518-4854-A0B7-A21098DB152A");
        public static Guid DecreaseVoteClout = new Guid("944EA446-A6EB-4E5B-B537-1504170AFD62");
        public static Guid IncreaseShares = new Guid("82D3E7F5-7580-457B-8C2E-273F396A1D5F");
        public static Guid DecreaseShares = new Guid("865BA138-3EFF-43BE-938E-2B30637E6BFA");
        public static Guid SetYeaThresholdOfActionType = new Guid("D49A8CCD-3BBB-40EA-A600-8523BB5B87F0");
        public static Guid SetVetoThresholdOfActionType = new Guid("AE567C9D-AEFA-46B5-88A0-A610AC7A4E4B");
        public static Guid SetYeaThresholdOfParticularAction = new Guid("D148DDD4-208A-4A3E-9D58-1C40DE00FDF2");
        public static Guid SetVetoThresholdOfParticularAction = new Guid("DF5162BF-CBE2-49DE-99D5-6CB5F6761E64");
        public static Guid SetMinimumHoursToCloseOfActionType = new Guid("9AC501C5-EA59-41CB-A463-54FB9449AD4E");
        public static Guid SetMinimumHoursToCloseOfParticularAction = new Guid("D0BF1C61-B294-48B1-A635-9E37E45B7A4D");
        public static Guid SubmitAction = new Guid("41B7BD73-060A-4E67-B313-A353E4FCE828");
        public static Guid CreateUserGroup = new Guid("{6EB561D2-E11F-4FB6-B3E6-006EF372E958}");
        public static Guid AddUserToGroup = new Guid("{F57F4207-0FB1-4BA1-A0A8-6CB95C148163}");
    }

    public enum LEDGER_ACTION_FLAG
    {
        None = 0,
        SystemAutomatic = 1,
        Privileged = 2,
        ExecuteImmediately = 4
    }

    public class LEDGER_ACTION_TAG
    {
        public static Guid All = new Guid("F5205854-75E4-4FD3-A334-4E7B010FF602");
        public static Guid NonSystemLedger = new Guid("EB31C742-B51F-4F94-B448-6D4FE49E62BC");
    }

    public class ACTION_STATUS
    {
        public static Guid Open = new Guid("{1B397CAD-0FAF-4D08-8FFE-253B508A6A9B}");
        public static Guid Executed = new Guid("{F00BBC31-03CA-43BB-8841-66E967E6CD7E}");
        public static Guid Failed = new Guid("{691D3EB4-24A5-48AE-873B-E3C62CF10996}");
    }

    public class PARAMETER_TYPE
    {
        public static Guid String = new Guid("{A947AFD7-1F0B-409E-A707-0EBB9F5777DD}");
        public static Guid MuitilineString = new Guid("{A57C7A1D-081E-463B-9597-04757AE0C71E}");
        public static Guid HTML = new Guid("{EA58294F-69D8-4781-8356-2D98A445D7C8}");
        public static Guid Decimal = new Guid("CCC07B74-C9F1-471D-B270-FF49C9D67762");
        public static Guid Integer = new Guid("F4B419DE-0546-4DC2-AF5D-0453740FADA8");
        public static Guid Bool = new Guid("18323D56-F9E3-4C38-BCEB-89E133F47D49");
        public static Guid Guid = new Guid("0E4D6BFE-9A1A-402B-A311-792701731FDD");

        public static Guid User = new Guid("{CD3EDB0B-48F6-470C-AF61-8F9BA94B474D}");
        public static Guid UserGroup = new Guid("{89A055F1-DE2D-4DAA-8801-87D9E0F208A0}");
        public static Guid UserOrGroupOrCollective = new Guid("{E374E830-120E-4389-A3E4-35D555FD36F6}");
        public static Guid ShareType = new Guid("{9DB6D175-FB5F-4EB1-B9B4-E4FE864F55C8}");
        public static Guid ActionOrLabel = new Guid("{F9DEE7B6-3416-4A3A-AAB6-722C0206FD76}");
        public static Guid Ledger = new Guid("{78B1F14F-AC27-4A0F-99F5-A71647A5CA6A}");
        public static Guid String1or2or3Caps = new Guid("{29F09322-4F1B-42CD-BAAB-6E50055AD024}");
    }

    public enum PARAMETER_TYPE_UNDERLYING
    {
        Integer,
        Decimal,
        String,
        Guid, 
        Bool,
        DateTime
    }

}