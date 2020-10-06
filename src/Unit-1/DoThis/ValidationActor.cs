using Akka.Actor;
using System;

namespace WinTail
{
    class ValidationActor : UntypedActor
    {
        private readonly IActorRef _consoleWriterActor;

        public ValidationActor(IActorRef consoleWriterActor )
        {
            _consoleWriterActor = consoleWriterActor;
        }

        protected override void OnReceive(object message)
        {
            var msg = message as string;
            if (string.IsNullOrEmpty(msg))
            {
                // Send signal that user needs to supply input
                _consoleWriterActor.Tell(new Messages.NullInputError("No input received."));
            }
            else
            {
                var valid = IsValid(msg);
                if (valid)
                {
                    // Send success to console
                    _consoleWriterActor.Tell(new Messages.InputSuccess("Thank you! Message was valid."));
                }
                else
                {
                    // Send bad input warning to console
                    _consoleWriterActor.Tell(new Messages.ValidationError("Invalid: Input has odd number of characters."));
                }
            }
            //  Keep processing messages (Actor doesn't care)
            Sender.Tell(new Messages.ContinueProcessing());
        }

        /// <summary>
        /// Determine if the string message is valid.
        /// Checks if the number of characters recieved is even.
        /// </summary>
        /// <param name="msg"></param>
        /// <returns>True or False</returns>
        private bool IsValid(string msg)
        {
            return msg.Length % 2 == 0;
        }
    }
}
