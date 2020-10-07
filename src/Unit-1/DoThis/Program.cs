using Akka.Actor;

namespace WinTail
{
    #region Program
    static class Program
    {
        static ActorSystem MyActorSystem;

        static void Main(string[] args)
        {
            // initialize MyActorSystem
            MyActorSystem = ActorSystem.Create("MyActorSystem");

            Props consoleWriterProps = Props.Create<ConsoleWriterActor>();
            IActorRef consoleWriterActor = MyActorSystem.ActorOf(consoleWriterProps,
    "consoleWriterActor");

            // Register Tail Coordinator Actor
            Props tailCoordinatorProps = Props.Create(() => new TailCoordinatorActor());
            IActorRef tailCoordinatorActor = MyActorSystem.ActorOf(tailCoordinatorProps,
                "tailCoordinatorActor");

            // Register the validation actor
            Props fileValidatorActorProps = Props.Create(() =>
            new FileValidatorActor(consoleWriterActor));
            IActorRef validationActor = MyActorSystem.ActorOf(fileValidatorActorProps,
                "validationActor");

            //  Register the console reader actor
            Props consoleReaderProps = Props.Create<ConsoleReaderActor>();
            IActorRef consoleReaderActor = MyActorSystem.ActorOf(consoleReaderProps,
    "consoleReaderActor");


            // tell console reader to begin
            consoleReaderActor.Tell(ConsoleReaderActor.StartCommand);

            // blocks the main thread from exiting until the actor system is shut down
            MyActorSystem.WhenTerminated.Wait();
        }

    }
    #endregion
}
