using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Models;

[Index(nameof(UserName), IsUnique = true)]
public class ApplicationUser : IdentityUser
{
    [Required]
    [StringLength(255)]
    [Comment(
        "ユーザー名. ログインIDにも利用するためシステムで一意. 補足：メールアドレスをログインIDにしない理由は以下の通り. 権限の観点から店舗スタッフと店頭購入者を明確にわけたい. メールアドレスにしてしまうと別のメールアドレスを用意しなければならなくなる. 記憶・入力の負荷を考えてこの対処は取りたくない.")]
    public override string UserName { get; set; } = default!;
}
