using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MeetingSystem.Utils;
using MeetingSystem.Message;

namespace MeetingSystem.Client
{
    public enum ClientMessage { ScoreMessage, VoteMessage, FileMessage, RankMessage, CloseMessage, CaseVoteMessage, PromotionMessage, AssessmentMessage };
    public partial class MeetingClient
    {
        public static void initMessage()
        {
            ScoreMessageHandler.init();
            VoteMessageHandler.init();
            CaseVoteMessageHandler.init();
            FileMessageHandler.init();
            RankMessageHandler.init();
            CloseMessageHandler.init();
            PromotionMessageHandler.Init();
            AssessmentMessageHandler.Init();
        }
    }
}
