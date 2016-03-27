This is the Quartz integration plugin for Akka.NET.


## Using ##
Install:
	```PM>Install-Package Akka.Quartz.Actor```
Create a Receiver:
``` 
class Receiver: ActorBase
{
 	public MyActor()
    {
    }

    protected override bool Receive(object message)
    {
    	//handle scheduled message here
    }
 }
var receiver = Sys.ActorOf(Props.Create(() => new Receiver()), "Receiver");
```

Create a QuartzActor:
	```var quartzActor = Sys.ActorOf(Props.Create(() => new QuartzActor()), "QuartzActor");```
Send it add messages:
    ```quartzActor.Tell(new CreateJob(receiver, "Hello", " * * * * * ?"));```

Now message "Hello" will be delivered to receiver every 5 seconds.

For more information, please see the unit test.

For more information about quartz scheduler please see
http://www.quartz-scheduler.net/documentation/
For more information about akka .net please see
http://getakka.net/docs/