using automated_electrical_schedule.Data.Enums;

namespace automated_electrical_schedule.Data.Models;


public partial class Project
{
    public Project Clone()
    {
        return new Project
        {
            Id = Id,
            MainDistributionBoardId = MainDistributionBoardId,
            MainDistributionBoard = MainDistributionBoard.Clone(),
            ProjectName = ProjectName,
            DateCreated = DateCreated
        };
    }
}