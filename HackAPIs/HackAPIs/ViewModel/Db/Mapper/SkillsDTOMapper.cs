using HackAPIs.Db.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HackAPIs.ViewModel.Db.Mapper
{
    public static class SkillsDTOMapper
    {
        public static Skills MapToDto(tblSkills tblSkills)
        {
            return new Skills()
            {
                SkillId = tblSkills.SkillId,
                SkillName = tblSkills.SkillName
            };
        }
    }
}
