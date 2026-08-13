using IdentityMail.Web.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityMail.Web.Context
{
    public class AppDbContext
        : IdentityDbContext<AppUser, AppRole, int>
    {
        public AppDbContext(
            DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<UserMessage> UserMessages { get; set; }
        public DbSet<MailDraft> MailDrafts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<UserMessageCategory> UserMessageCategories
        {
            get;
            set;
        }

        public DbSet<MessageReport> MessageReports { get; set; }

        protected override void OnModelCreating(
            ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // UserMessage - Gönderen kullanıcı ilişkisi

            builder.Entity<UserMessage>()
                .HasOne(message => message.Sender)
                .WithMany(user => user.SentMessages)
                .HasForeignKey(message => message.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            // UserMessage - Alıcı kullanıcı ilişkisi

            builder.Entity<UserMessage>()
                .HasOne(message => message.Receiver)
                .WithMany(user => user.ReceivedMessages)
                .HasForeignKey(message => message.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);

            // UserMessageCategory - Mesaj ilişkisi

            builder.Entity<UserMessageCategory>()
                .HasOne(messageCategory =>
                    messageCategory.UserMessage)
                .WithMany(message =>
                    message.UserMessageCategories)
                .HasForeignKey(messageCategory =>
                    messageCategory.UserMessageId)
                .OnDelete(DeleteBehavior.Cascade);

            // UserMessageCategory - Kategori ilişkisi

            builder.Entity<UserMessageCategory>()
                .HasOne(messageCategory =>
                    messageCategory.Category)
                .WithMany(category =>
                    category.UserMessageCategories)
                .HasForeignKey(messageCategory =>
                    messageCategory.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // UserMessageCategory - Kullanıcı ilişkisi

            builder.Entity<UserMessageCategory>()
                .HasOne(messageCategory =>
                    messageCategory.User)
                .WithMany(user =>
                    user.UserMessageCategories)
                .HasForeignKey(messageCategory =>
                    messageCategory.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Aynı kullanıcı, aynı mesajı aynı kategoriye
            // yalnızca bir kez ekleyebilir.

            builder.Entity<UserMessageCategory>()
                .HasIndex(messageCategory => new
                {
                    messageCategory.UserId,
                    messageCategory.UserMessageId,
                    messageCategory.CategoryId
                })
                .IsUnique();

            // MessageReport - Mesaj ilişkisi

            builder.Entity<MessageReport>()
                .HasOne(report => report.Message)
                .WithMany(message => message.MessageReports)
                .HasForeignKey(report => report.MessageId)
                .OnDelete(DeleteBehavior.Restrict);

            // MessageReport - Şikâyet eden kullanıcı ilişkisi

            builder.Entity<MessageReport>()
                .HasOne(report => report.ReportedByUser)
                .WithMany(user => user.MessageReports)
                .HasForeignKey(report =>
                    report.ReportedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Aynı kullanıcı aynı mesajı yalnızca bir kez
            // şikâyet edebilir.

            builder.Entity<MessageReport>()
                .HasIndex(report => new
                {
                    report.MessageId,
                    report.ReportedByUserId
                })
                .IsUnique();
        }
    }
}