using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using WebApiMyBgList.Constants;
using WebApiMyBgList.DbContext;

namespace WebApiMyBgList.gRPC;

public class GrpcService(ApplicationDbContext context) : Grpc.GrpcBase
{
    public override async Task<BoardGameResponse> GetBoardGame(
        BoardGameRequest request,
        ServerCallContext scc)
    {
        var bg = await context.BoardGames
            .Where(bg => bg.Id == request.Id)
            .FirstOrDefaultAsync();
        var response = new BoardGameResponse();
        if (bg != null)
        {
            response.Id = bg.Id;
            response.Name = bg.Name;
            response.Year = bg.Year;
        }

        return response;
    }

    [Authorize(Roles = RoleNames.Moderator)]
    public override async Task<BoardGameResponse> UpdateBoardGame(
        UpdateBoardGameRequest request,
        ServerCallContext scc)
    {
        var bg = await context.BoardGames
            .Where(bg => bg.Id == request.Id)
            .FirstOrDefaultAsync();
        var response = new BoardGameResponse();
        if (bg != null)
        {
            bg.Name = request.Name;
            context.BoardGames.Update(bg);
            await context.SaveChangesAsync();
            response.Id = bg.Id;
            response.Name = bg.Name;
            response.Year = bg.Year;
        }

        return response;
    }
}
