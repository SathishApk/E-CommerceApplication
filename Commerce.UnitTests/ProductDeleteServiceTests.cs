﻿using Commerce.UnitTests.Helpers;
using CommerceApplication.Models;
using Microsoft.AspNetCore.Identity;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commerce.UnitTests
{
    public class ProductDeleteServiceTests
    {
        private string DbName;
        private string UserId;
        [SetUp]
        public void Setup()
        {
            DbName = ContextHelper.CreateDbName();
            using (var context = ContextHelper.CreateContext(DbName))
            {
                var roleList = new List<IdentityRole>()
                    {
                        new IdentityRole { Name = "Admin" },
                        new IdentityRole { Name = "User" },
                    };
                context.Roles.AddRange(roleList);
                context.SaveChanges();

                context.Users.Add(new IdentityUser { Email = "sathish@gmail.com", UserName = "sathish@gmail.com" });
                context.Users.Add(new IdentityUser { Email = "sathish123@gmail.com", UserName = "sathish123@gmail.com" });
                context.Users.Add(new IdentityUser { Email = "sathishUser@gmail.com", UserName = "sathishUser@gmail.com" });
                context.SaveChanges();

                var userRole = new IdentityUserRole<string>();
                userRole.RoleId = context.Roles.AsQueryable().First(x => x.Name.Equals("Admin")).Id;
                userRole.UserId = context.Users.AsQueryable().First(x => x.Email.Equals("sathish@gmail.com")).Id;
                UserId = userRole.UserId;

                var userRole_2 = new IdentityUserRole<string>();
                userRole_2.RoleId = context.Roles.AsQueryable().First(x => x.Name.Equals("Admin")).Id;
                userRole_2.UserId = context.Users.AsQueryable().First(x => x.Email.Equals("sathish123@gmail.com")).Id;

                var userRole_User = new IdentityUserRole<string>();
                userRole_User.RoleId = context.Roles.AsQueryable().First(x => x.Name.Equals("User")).Id;
                userRole_User.UserId = context.Users.AsQueryable().First(x => x.Email.Equals("sathishUser@gmail.com")).Id;

                context.UserRoles.Add(userRole);
                context.UserRoles.Add(userRole_2);
                context.UserRoles.Add(userRole_User);
                context.SaveChanges();

                context.Products.Add(new Product
                {
                    ProductImageData = Encoding.ASCII.GetBytes("0xFFD8FFE000104A46494600010100000100010000FFDB0084000A0708161515181512151818181A181A19181A18191A1918181A1A181A1A191C1A191C212E251C1E2B231818263A262B2F313535351C243B403B343F30343531010C0C0C100F101E12121E342B252B3434343434343434343434343434343434343434343434343434343434343431343434343434343434343434343434343434FFC000110800A8012B03012200021101031101FFC4001B00000105010100000000000000000000000400020305060107FFC4003C100002010302040306050303030501000001021100032112310441516105227113328191A1B1064262C1F052D1F12382E11472A207156392D233FFC4001A010003010101010000000000000000000001020304000506FFC400281100030100020202010207010000000000000102110321123141510442811332617191A1F133FFDA000C03010002110311003F00F1BA54A95323855D06B94A89C4A8D144A5DA081A951AB9312A430DCA8D9A9034E4B7354ED92C4827C2F86172EA5B2DA43BAA96FE904804D5B37E1B3ED6EA170A96D80676049C82C3CA37300C89E54FE0FC210B2BDBB9E410CFAC6974823503186EBAB03AE9AD078AC6A0CA118B8D48D71D422E416842558C95CC13EF0C08A5A6D12FE26D62F453BBA2DA29615B480195E4172E1D4B4E9077D2B1B00077AB9F07E3FDAA4139506577D4BCC4CFBCBEF03CB23354372EDCB6469544D3B04C2932A646931EBB73C54FC3B3B5E172D80A5986A13883904B4696226313D7D1293690DD26CD3F0EF0C6DB194683D0898323E6302667B0A0F88B8E8EA4192A74BF9A4B216210CE49607CBBC9D417383565C75AD68AD10E820C1C472F51BACFEBEE69D60ADD015C038821B12ADE52B263A8F4240E535245D21DABCE2F0FCF0B73E70AF8C1824068E4FBC0A27C5D0B5B06189B6EAD71672F6F4947208E6031711F99011B0AAAF06E340D56DF2C8C54CFE60254C8DE0891988D4A3724D5CEB2ADA492481E56DF529DA7FA88898E7A639E4E0C90570AABC4D97B17F24A947E5AC11E5B8BD254ABE3626395787F1BC235AB8F69FDE47646F552448EC627E35ECF25192E2C4615BA804C29279A86246AE60A9D85617FF52B808BC9C4A2F92F200DDAE20D241E84A85F5D2DD2A90C0D18D9A534D26BA2AA0254153A0A85289B62AB289D32545A22C5A2C614127A0FE6077A65AB64F6EA4EC05188F234AF9579F56EEC7F6DBE39ABC499AEBE83783B2A3DF7F8279BFF002DBEE2AD2D7B253EE393FADD57E98AABE1CC6171DF9FFC0F4A3F872365C77E7F3E42B4CE1E7F2EBF92D6DF0E1C4A81E90BF48C1F8C0A0B89F0E232A3B119C1F43907F6CD4B61B4E44CF23CCF7EC3D33DEAEBC3388D662E004C7BD00102701B91EA28D330DDD71F6BB451DBE14372C491F00267D706A47E04479472FF008FDAB42FE1A01214000C6D8DA67EE0547C69B76960CEA89D2A6091CE58EDF0CD2EA22BF25D562323C4F871D8983F7F4CE6ABB89F08B806AD248E64038F51CBD76AD35FE2F508450A0F21B93D098DFB919FA555DCB8C3CC84823A1F30F9E08F48AEA499E870F2D94C0845C6F553744C935A3E36F0712E037EB1E571FF0071E67D718F953719C3E90181D4A7623AF304726ED59EE70F4386BEFD958FDAA02B4595A8AE18DAB2D236CB0665A8E9ED5CD3532A816952A5531C54A952A28E3A054AA2ADFC03C0CDF2599B45B52033609131B2CF799AB6B3F869502FB56662CD116C8C2F9BCC49067DD26239198E54986FB33DF3C4BC6FB33DC3592C428E640F9E2B5E969386D3A551DFCBAB56821710C6499CB62390F8CB5782E1902310E8400C591C157F3608D630DE840104C74B574E0E10AB0132A4E4B2B03AA0AEA20100CE327BE299B94B1FB2154E9A6BD027887881486B6CAA750D30010D2CDBAAA891E5D3B0904F302A3BB7ED5CB692BA618C2130A8201708C375C4C76031C84F13F04372E7FA772DC28F2042DAD900D4CD1A40246A24A8322768CD338E65B56D5566234A99F364993D8925B3EA052B4AB5AF8156CE2F97FE8B7E1EDDA30AC8BA230DA34BEEC1497265872DB9C1939A0BC39ED17BDC3DC2CAF908EC04AB027CD3862B88227001A670BC692A1B524880EA1C18985274810B3F9883008530208A1BC591A5789B609D3E5B82396D2475DBE439CD435E9AA6161A5F09E3D80297079930CA7F3291B038953C8E009D81060E50A86572A4C83D54CEF8EEC36EA776159D5B9AD55D3CCEA361F9D372013BEF233BC4406336BC0F1C0AF986073CCED27501D4093B7B82249A0CAA44BC7A28BC2E031A9439C81304ABB88CAB29235740EB0495336C6F109D587996001239E9E98823A62A9F8BB05D00B64EA53AEDE60AB0C159DB4912A4FBB94603A4BE1FC6074859E4CA06083B9503973209DA571232462EB85E255C0681A0813D33E533E8481E9E952F15E1E97D1F86BD94B8B2A47BC0AECE9DC6FF0030704D01C3DD2A4408572640D83010C14F4312B3C883D6AC6DDC38244C182477FCC3B607C3B8AE96068F15F18F0C7E1AEBD9B83CCA70793AFE565FD2C33DB6DC1A0857B37E2EFC3E38DB50B02FDB936DB03503328C7FA4C60F223A135E36E841218104120822082304107620F2ABCBD1092D9A2ADB450686884356964E9168B74469F9D4CB1B0FE1A02C1A2ED9AD12F4CB5381B64D5A70A93BE3A9E806F553C39CD5B2380BFCFE74AB4B30F326585B7CF9401EA0131EA7F6AB2E0DE7700CFC0E4C0C8E83AF5AA7E1DBEDF7C7EF56B61C01F4FB0FB06AA2ECF379A7A34BAC0B20C89D813C86331F1159CE20831A813DC9CF9890DF7157FE23C368B018EA00811318DB73F0ACBF17772639FF00706A5C798DAFB11452A49ACE97FD07D00E1673883BF683FCF8D56714DCC60FEF455EB90C63AD05C7BE49EB9F9D16CDFC52F4AEBCF1246C771CBB8A05AE413CD5B04751FF00E8723FDE88BA684D3323E23F9E9F6A9533D1849207BF6F49EA3707A8A06FEF47DE6C474DBD39FEDF5A02E0ACD66CE32135CD55D614DD35065902D4A96E698BBD5CF86C1201008A68954C5E4BF15A561B26A3648AD871DE00CA0328F29122B35C6D82A608A6BE27288F0FE44F27A619F86FC47D85D1ABDC6857EC3AFA8927E75B77652590992BB1C48565C107683247FBA39D7988ADB7805FF69695865ECF91979BDB3EE8F84103A695EB4DC35FA487E6712FFD111788A4A9F29F2B173910BAE5893E9E5CEF20F7A06D710ACA51F037431EE90654FD5A779E7569E2C421D532ACA4123F323ECC3D3EF3DEA838FB402EA43207BC3A6A3E523AA9DA7D3BD2F34F63F037528B7B6D724323609128C7C81D46F3D70086A778A705ED50DC4045C4C38204B1E6C20019FD81EC2ABC3B8C3A627E273B1E7D2319E93D6AD53886422EA10001A5C40F77AB0320883D8E24F2ACBDA35292BF80B8E14B32B136E49C7E43EFE313133E922AEF83E241324F92E001F38CF94C9E864093CF49C49AA3E26FB70FC4B69F75BCCBD083F7D883E9532DDD0F16E3436429981ABF2FFDB92BF1EC683EFB1D2C2EACF05EC58AA99B6728DB1126609E464347AF2326AEF86E1C34BA0DE3DA272DF0EBD339F5326A8F81E2FCDA79C480DEEB29C47C4E9DB99040DC9BDE05C020A7B9991FD27F30C72F5EFB5118E5A4F66E430F2C4410768227E5E5EA46918934F3C06972E1A15E5C1E8DBB6D8927CDEB8AB64456035093C9819C1D8C8EBD7991DC8A80A694D2A44AFE5E9D20743CBAC93BD28475B6810DB1898FCAC261BB447FF503B517C35ED332223EDCC7A0271D430E98AB5E28150C70444FCF71FD447964F331BE69FC4BECE39E2373A863E262567B4D700978DF1936C816CA992402D2446E0C0DCC8C091802B01F8CB87FF57DAC826E02EC546905818274C983113D48279C0BEBF635BB37B37BE5448B48DA1540C17B9731A44880241241DF967BC66EEA55428C86DB3028EDAD935AA980DBB290A184CEE7271578275D19F5A9D5AA13BD4895591185D96A2D2E8A0D205382F3AB2A2352996361F356365E71551C398A395E2AD3464B82E2D3C73A36D5C1A94338513926481B9D80279D501BB037CD756F999FBD3AA325706B37BE31E2BAECAA8B88614123519620C7944762731BD65EE713B7A543C6F88EBD3E444D2B1E4D59F99803A0EFCE827BD5C9A4B10170BFD5D853DE9627BD09C537DA9EAD8A1AF34D2B65E23B02BA68767A91CE687BC7A54A99B2103DC35038A96E2D374D468D320C4536A5714CD1536874C094D5A7865E50CBAB69CFA555D383D2CD78BD0DCAA586BBC6BC7BDA3426154428F4ACF7137F50CE684F686985AA97CCE89717E3CF1AC48ECD59781F889B174364AFBAC07353BC7704023B815575D4A8A6D3D45AA554B4FD33D238DE155ED1D30CBEFA11CD1B263D0998E86B292C874882575100ECE87DF461CC4437604F302AD3F0978948F60DDCAF707DE53F7F9F415CFC43C01587B7BA995F4FDE323FC56CACB8548F3F8B78F91F1BFD8A3B83D932BA4946F32CEE39153DC1953D6279D5EF01C402BA9338CAEE597D0EEC276E727A88A9464886C5BB991FFC6DB11D6371E8279CD06CAF61E3220C83C8F420FF0039D63A8F946F97F0CB1E3DC328D3936E1D0FF55B982A6732A637E866B97D1885740082363919123E1BE3A18E75D3C4AB85BA34E1A2EA9DBCD82D03656EA3634ACB1542A77B6E77DCA4CF2DF19F82F5A994084E29880632323F521C329F49613D08DB1579C0F1F10EA64E27A9DB70799057E240C1234E6AE3106391CA9C63A83CB3D3B472A7F0FC4C6260FA7D0E6799CF30CDCC820E1C6DB88F164450DAC2C6C32499DE00C9689EE609E79A9B9F88C3B4C111B4E2419DFE64769ACF8F0EBB754DF664540635DC6D2B3D1073FF0081D2ABAF965C875751CD09C7A82011F6AE528E36F6F8F2C37F78996E87306397E63F114727145A081EF007F9D369FF007562FC238A3EEEF998EA67ED5ABF0F5E60E267B9E73D864C0EEBD283581450713F88B88E1EF3AD965505C379911B64020961EEF38EB9AB5E03C3AE71E976E340B82DDB0B1015EE2162076942A3B123955BF11F85ACF10DAC2AB16827CEEA4E00995241D80F77E26AF3C33C3DED0540111241D285D9DCEF0CCDE94F35D60953F278D3AE6082330411044722391A95600AD47FEA2F872DBE27DA5B002DE52EC06DAC1873F1953EA4D6435D5D325EC203E6A4172A05A92D275A6D03483F8624D1FE55424B66AB3FEA748851F1A83DA1A75586770E9E8725CCD11AE2ABADB453CDD24D32A16B8F4B2F6D5D404ED55C2E517C33B6C29950951883E61681BCE454F71E373405FBB35CD822461339A86E115D069CCA395232FE81181A85C9145B89E743DE8A4A4565900534B4D4D61669FEC693C42DA4CA7029114E02911512E340A44576B84D71C72BA2B829D5C7326B170A9041820C82391EB5BEE0B8F5BF6F5699270EBFABF308E8D33F122BCF0559F847889B2D2320FBC3A8FEE24C7C7A9AAF17278BC7E999FF00238BCD6AF6BD06F17C005257F239943CD5B61BF5DBD6282B57601B3776D95BFA0F23E9DBFE2353E23696E206482184F49EE3A19C1EF18AA0E3ADAB096C11863D27627B1EBC882394D3F24E3D42F0F2792ECAD0AD69C86D88D2D190CA778EBD4771442DC2A412768563FD4BF91BD46DF114FB0B234B990BB37E926241FD2C47CC729A16F4A9D0DB6DF0359EA7E51A957C0597D2206C320EF8E51F6F877A6A90E41DB206394919FDE844620E927FC75A7DA241C030324741327F734B83171C3F8FA25DFF0056C2DFB68BA111CE106CCCAA4152DEA3A77975FE0ECF13795B85B6F6ADBB4146CC733A48E440381B6282E17C01EEC14BB6083CDAEAA9FF0072B0D40F68AD6783787DBB0852DB8BB748219D336ED83BF9B9B1881F3C464EA48569EF454F84784C30813B63ACE07EE3E24F2AD08E1F4E3975F8EE3E608F515370F6574C7E6D819C8938DB94E3E58C79673257FD4209F781182483931D77EC361CAA6CA209F0BB8C8A67ACE7BEFB7724C0DA6893E22491390490733DC48EB8FA1355F66E08C11B18CE369DF90220F580BD6A1E276C48CEDB644188E4440F9D72057A20FC75172C2B73571F105581FAE9F9579D9014E735B1F1EBE7D8113BBA91E9048FB562AE6F5A57F299E7B6C905E35221A19688534530B44F1D29D6D26A34153DAAA2255D129B102A13452BCD3911665A9BC49F9610584934497838AE3DCFE902984127BD3A15BDF63AF1268600F3AEF14AE8449DE95BD4EA6297E464B10E68A61314C4046F516A2C683632448CBD2A07B7268833B0AE45068E4F08408A76BA4E94D9A181F6550A4D5CA44D66349CA6D4816BBECEBB03A454E069CC94C8AE38ED741AE034AB8068FF000F78805FF4EE1F2B1C7E96FEC7EE01EB47F89704079865482080371CC4758C8F42399AC9D978AD4783F1E1C1B6ED0705493F98743D7EF1F3D1C74A978B3272CB8AF39FDCA54B654889239626562248E783040DC308A7F1BC38280A998F74F558983DC63D66798AB2F16E18A640D22797E46C9C74C9623FEEED5556B8B8255B62723973CAF4C9240EE47489D4F8BC65E2FC9268024FC451BC1713A082699C670F1E65C8EA398FE7D2848A9B5F05534CD5F0F72D3417B68D1BF97247A0DF9E39C1033BDD8F100AA16D8545030142C0E7D239833B1C122188381E1F892BFBF7FE75F4E9561678B239C8DC76DF1F53FF00976A5C18D77FEE23778DCCC73C6F07E133B82264EA25EFC586064927AEC4FAF53CB39F77A565178AE538E5E9DFEBF3A96DF131FCCEF1F7FE6297C43A68387E30EA8279E0F7DCC0EBBFC80E552F1DC66A10997D24C0C9200927D00DCF5AA006EB130E8831E663A9A3A8559E9CE28BB1C4222E8B5A999E03DC600330927488F7579C0EB452EC56FA07F1ABBE445DA64FCB03EE6B36FBD5AF8DDF97206C303EE7EA4D548CD5FE308AFB10A212BB6AD667957788227CB4C961CEB59D98DAA743D6A14CD48AA69D0944E2E32F2AE8B7AB99EF4D553B9A215A70053A254F078B51B1A6FB4D26668EE17C399C8CE2AE93F0E230804CD5543665AFC889794CC8711703996A92D5C502018A97C57C3CDA7287355E5691A699A67C6A567A0B0808A88DBD3B576DDB7482E8CA184AEA52030EAA48C8EE293BCD03BB4F08DDAA32F52115138A183A384D374374A9AD5B9CF2A7FB5AEF1FB3B5AF45054B62C33185049A62249A32D5C230B81FCDEB344A6FB345534BA0AB7E0F7224007D335D5F0C7274E933B4466689E06EB2C10C564F2DEBD07C2EFABD87B8402E8A34B464CF33DEB64F14BF48F2FF0027F2B938BBC4CF3DBDE0A104DD6D3DB9FCAAAF88E1147B8D3EA22AE3C41C33313392724CE7BD52DEC54B96657586BE0AAA5AD8195839A54E734C35959ACED489748A80D741AE0B934BC278AEB5D370CE34C9E63A377E8DE940F8870910CA641D8F5FF9AAC56228BB5C6B69D0D953C8F2F4E954F3D58C8FF0FC5EAFF03F86BD8D0DB1DA7AFEC73FC99A65EB119FE11D4575AD12A580951B9E636C9F98CD316F488C903D71EB4AFF00A8EBED1132669E8D14B54D214A312ADDFE7D7EF532BFF3D28614ABB03A1DAFE4627BF2A3386BA14176E59FDA05531BB119DAB8F7C9C72E9452056B25E22E6A249DC924FA9A65B315186A72B53A62E7414D7CC46D518239D71335214C53FB13D095BA5101EA045A9169E4144EAC6AD7C3F8527CC62AB2C6F57FC070ECD1D2AD0B4C5CF7E289384725E3333CB6ABCF12E316C208F7C8C76AE59E196C21B8FF0001D4D64BC4B8C6B8E598FA555BC314C2E6ADF840DC65F2EC598C9352786709A9B5B89453907F31E4BF627B7AD77C3BC3DEFB845DB7668F7475F5ABFF0014B2B69022290AB204F332413F399EB9A8B6B4F47D2C452F89F1AD7201321663B4EF1DBFB55795A9D96A32B470E968858547A7AD5E786F87F994BA172D1A2DE7CDFA9A33A7EFE9B8DF8838744B842321244BAA214546D8A00499DA79649C0A57ECA2657CE3151C5385726B8052AB5136DC50334F46AC53586CA9D2E6CDCDAB5FF86FC5D56D5F4612593CBBE08E78AC05BBB53AF1046C62B5C72E1879BF195AC617C55E924D565F7A73DDA19DAA3C97A69E38F11A5AB934DA550D2F83ABB14CAECD13B07574570549690B1006E4803D49815C2BE8B5F0A602DDD2DB003D0EA9047D14FC0D575D5D2D28641FA8EF5AAB1C0AAD9D05664E796A63B02476031CB4CF49CE78858F6663054ED55B9732B48F1DAAA78046395206B869548D087B481BD19C2042DA6E6A0081A48E46624CD04FB515C2492622346933FA8E3F9DAB8EC0BE37C2DD0CAF9D3FA946476651257D763D6A33E1B742EB6B6E17FA8AB01F1318F8D5D70175D5967524004395206989F374318DFAEF88D4D9BA85031BB70640D769C955E92A0E9CE71127EB5C9FD81AFA3CD7D89E54D2915BEF15F03671ACBDB2E7DD75508AFD03E91A437EA27E240918FE27876462AE0AB030411041E841DAAB2935D1274D3C60A98A954517C4F08AB66D309D6FAC9E9A55CA831CB223E0684414C04F7B27B2809A938A50088A851A2A55704ED545E89D6EE8EB42B51E09E2C88BA6E09E86B3735220AA4BC337371AB58CBBF1BF14375A06146C2AAEC70CCEDA54124EC051DC2785BBE6227699963CA077FF00135A0B1E0168DA63ED995C099560A7B081F63346A92F60E38C5E324FE0C89C3284D3A9DB3BE59B607B28FD8F33893C6BC3CDDB2A2D8D4DE59EB82756FDDA683E038C0996B16ED89305CB976031AB4923CBFA898EF47FFD7DAFD50409963A4F75562E0FA8A836FCB4D2A578E32813F0DBE0DC744071B5C633D234893D8524E1786B30EC5AE8EA069CF55560703B833CAAE388F1744045B0AA24131D46C797D87F7CA715C4FB4792607F36FE75DB6AA4F957B26D4CFA2C38AE3BD94FB363ACE756CC0476E707F7E959DBC4B316624924924EE49DE89E25E4D4056A8A520790394A669A28AD374D77887C8CB57453F4D708AF38DFA2069DAAA3A55DA760F269A4536BB35C76088A54A6B95C13B14E54A6A9ABAF09F0A6BB07652624EE63781CE28CCBA7885BB52B59CF0FF090D05D946ACAA065D6D3B60FBA0F7C9E436A257875B4D85F30EBB88DF7DBD4C55BBF87DB45D2155800258A8249DA3EA3B6464CD57710B9196C89E44E239B648C75EB54791D676669A7CADBDE89178DD2A5988D880276D5EF1EA7119EF18F2E9A2BDC46A6D4DB0C81F6FB7D4D163856B8D127481A8C8C000FC277EBCE997386459FCC7B9C08EC00FAC52D53A2D30A7D15CC0198F874A61145DDB3F99447519FA5456CEAC6C791A42A88C2931565E1B64380871AA493D6013CBD281652009EBFB7FCD5A7855B0BE727001FA883F49F9F6AE0B2EFC23C3CA49477040312752939D2749181E53F3A5C5203AD1A2DBFBC1D2555A2725673CF3BF2E94570F7C69246FB4F688F9E4C8EC69FE24AB70B5BC03A199588983ACE63BCC7A570007C378AE2EC9303DA208D417CD03BA8823D0E288E2AEA712FE73A58E02DD041EC16E46A5DFF0036A51B5677C3F8F65234BB21E466541F4271F022B509E39774C5DB42EA44CA01715BB956DBD7EB4CB50949302FC4FC42B15B6B605BF661564FBF0AB0167FA44CE30492DCEA802D6C5DF86E2947BCAEA2014CE88FCAC8F98EC18C556711E017172855D7265485200FEA56208F848EF569699069C94C9D29EB6B3451F0F70BAF412A37658651EA5663E346701C1CC33091C976D5FD877FF22A913AAFA23F0FF0D6B867DD41BB9060761D4F6FB55C70FE1E8B8B6ACCD83ED1C69503A81D3D351EE28BF6A5001E524C05000D236DA4663D318DBDE23789F1E10416D47F31FEA3DFB7F8E46BB7E85F16FD92DFBC10121E4C65A33FED1C86FDFD7735177C41F2D6C6E739F74CC9C4C9142B5C67F33485E43AFF003F98AEB41C72E836A2A74ED5258DCE2CB0D710E0F9960E7F5827233C875A87FF007173B003BE667A8A1355745514A11DB1CEECDEF66B9B5740A7E8A6484744114B45122DD4AB669B0476900FB3A5ECE8F7B51B545A2BB0E5C9A62A69869A1A96AAF24F630E9A61AE935CA0C2854ABB5CA21152A5445B508E3DA29201F32C9424749891F2A006F08457A658E182285E4B6ADAE37248667F8922B167C3D1C17B0CC601251E35A81B9046187C07A56E7C42E0031BB1527B4A80807C467D7B569E0E93679FF94FCEA657F5D2B6EDD05803FF007103FA479444ED243E3F48E555896817668F2E91A700480635907912498C6E37C0317882238B970930AAAA904C18283E3BB7C66A1B05EE010A5506EC601303F2F53CA7313BC567BAF266CE285288AE3B0621479581E73911392463033F3A5A7A8F9FFCC47D0D49C4DC50FE5D80D2A412390D59EFCFEF4F549E781D23036C9D87AFCE80FEC15801BE3967F9F4EFCEA0B9C091054E49181B004489E727063A6798AB0BD6C20C01F11FC3BE3F6E75DB7332DF33EF19DCC6D9CFF89A1A100B4932A70E84C7AF4FB474A9FC32F82BA184E7B0804EF9EE7E953711C18604A2C36E0F3C773D73F7A0782797F28CB0E5CE727D060531CCD0A5C24855C2E0998E400FE7AD31B8A09750B481A1A473C677F852B073B74F8F3FAC9A0BC5AE79DBDDF2A4409D5E6D26768DBA5195A236545D4C93DE71CBD2A5E1F8A74D898DF1FDAA12C69C8F545802E13C5C3002EA878D8B8063D09048F851FC07100906C5CD2C083A18B153E877F81C5505BB906518A9EC7F6AB1E1BC46F03FFF00661F2FED8A6F1FA037F66B4F14EC90E85198CBB2B05D6A046925791244F6522843C4059D876E807D40C7AF4ED5D6B8A32D2C4EDE6762C7DD1CCFC76FF3DBCE0885EBF127BFD28CA623482AD5F2355C6304C84DA1476EFF006FBD6170EC59B3D3A7AD477EE6A3A670373D7B0A6979DB0390AB4C92AA257BB35C534D449A942815548CF4C4A2A455A4A6A644A6489D51D4B752AA53D56A554A7488D5918B74E54A26DA4E2A43669B08BE40464A8FD9D5968C4533D876A3805CA794D2A54ABC43E9C54E06952AE00B14A2952A080C40568B8AE07DBE9BCB76C8D40060CE03061D572768CF634A95127CADAED04F82F006DDC0FA91D44C94752063A183F4A97C6AEBDCBC6DAEA51325BF4938D279882B9A54AACBF94C72F79FBFA23F112111542C8F7748EAA49F8FBC283B659DD51BCA58E9807DC53B931F9A271CBB6052A552FD48DDFA5FF622E2AFAB1D29242C4FF4963D003C848DE9E8630B9FAFCB97389AED2A1C8DF906524BA38F21817C60C73EF9EBCEA3FF00A8D47CA7CA36FD4799F4FDFAD2A54AC641D6388881E84FC318FA9A8784E182DE738F758819C06236EC3CC3E1D6952A28258D81263AFF00267E155FE3AE217196D4679C6001F4FA52A5549235ED14E2BA052A54C82C910D4C8FDBF6A54AA92230917720E76FF14F5BE7614A95511363854B6AD934A95524857A09181496952AA112645A2ECDBAED2A6923C8DE05A70E6A64E16952AAB3155B274B0472A915734A95124E9929E1C988A3ADF03814A95033DDB3FFD9"),
                    CreatedBy = userRole.UserId,
                    CreatedDttm = DateTime.Now,
                    Owner = userRole.UserId,
                    Price = 20,
                    ProductName = "Toys"
                });

                context.SaveChanges();
            }
        }

        [Test]
        public async Task DeleteProductTest_SuccessSenario()
        {
            var actual = false;
            using (var context = ContextHelper.CreateContext(DbName))
            {
                var productService = ContextHelper.CreateProductService(context);
                var IsSuccess = await productService.DeleteProduct(1, UserId);

                if (IsSuccess)
                {
                    var result = context.Products.Where(x =>x.ProductId==1 && x.Owner.Equals(UserId)).FirstOrDefault();
                    if (result == null)
                    {
                        actual = true;
                    }
                }
            }
            Assert.AreEqual(true, actual);
        }

        [Test]
        public async Task DeleteProductTest_FailureSenario_InvalidProductId()
        {
            var actual = false;
            using (var context = ContextHelper.CreateContext(DbName))
            {
                var productService = ContextHelper.CreateProductService(context);
                var IsSuccess = await productService.DeleteProduct(2, UserId);

                if (IsSuccess) {}
                else
                {
                    actual = true;
                }
            }
            Assert.AreEqual(true, actual);
        }

        [Test]
        public async Task DeleteProductTest_FailureSenario_InvalidOwner()
        {
            var actual = false;
            using (var context = ContextHelper.CreateContext(DbName))
            {
                var productService = ContextHelper.CreateProductService(context);
                var IsSuccess = await productService.DeleteProduct(1, UserId+"_");

                if (IsSuccess) {}
                else
                {
                    actual = true;
                }
            }
            Assert.AreEqual(true, actual);
        }
    }
}
