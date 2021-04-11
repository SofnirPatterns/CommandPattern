using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandPattern
{
    class Program
    {
        static void Main(string[] args)
        {
            SimpleRemoteControl remote = new SimpleRemoteControl();

            Light light = new Light();
            SwitchOnCommand switchOnLight = new SwitchOnCommand(light);
            SwitchOffCommand switchOffLight = new SwitchOffCommand(light);
            Door door = new Door();
            SwitchOnCommand openDoor = new SwitchOnCommand(door);
            SwitchOffCommand closeDoor = new SwitchOffCommand(door);

            remote.SetOnSlot(0, switchOnLight);
            remote.SetOffSlot(0, switchOffLight);
            remote.SetOnSlot(1, openDoor);            
            remote.SetOffSlot(1, closeDoor);
            remote.OnButtonWasPressed(0);
            remote.OnButtonWasPressed(1);
            remote.OffButtonWasPressed(0);
            remote.OffButtonWasPressed(1);
            remote.UndoButtonWasPushed();

            Console.ReadKey();
        }

        public interface Command
        {
            void Execute();
            void Undo();
        }

        public interface Switchable
        {
            void On();
            void Off();
        }

        public class Light : Switchable
        {
            private bool State;

            public void On()
            {
                this.State = true;
                Console.WriteLine("Turn on light!");
            }

            public void Off()
            {
                this.State = false;
                Console.WriteLine("Turn off light!");
            }
        }

        public class Door : Switchable
        {
            private bool State;

            public void On()
            {
                this.State = true;
                Console.WriteLine("Open door!");
            }

            public void Off()
            {
                this.State = false;
                Console.WriteLine("Closed door!");
            }
        }

        public abstract class SwitchCommand
        {
            protected Switchable SwitchableItem;

            public SwitchCommand(Switchable switchableItem)
            {
                this.SwitchableItem = switchableItem;
            }

            protected void SwitchOn()
            {
                if(this.SwitchableItem != null)
                {
                    this.SwitchableItem.On();
                }                
            }

            protected void SwitchOff()
            {
                if (this.SwitchableItem != null)
                {
                    this.SwitchableItem.Off();
                }
            }
        }

        public class SwitchOnCommand : SwitchCommand, Command
        {
            public SwitchOnCommand(Switchable switchableItem) : base(switchableItem) { }            

            public void Execute()
            {
                this.SwitchOn();
            }

            public void Undo()
            {
                SwitchOff();                
            }
        }

        public class SwitchOffCommand : SwitchCommand, Command
        {
            public SwitchOffCommand(Switchable switchableItem) : base(switchableItem) { }            

            public void Execute()
            {
                this.SwitchOff();
            }

            public void Undo()
            {
                this.SwitchOn();
            }
        }

        public class NoCommand : Command
        {
            public void Execute()
            {
                Console.WriteLine("No command");
            }

            public void Undo()
            {
                Console.WriteLine("No command");
            }
        }        

        public class SimpleRemoteControl
        {
            private Command[] OnSlots;
            private Command[] OffSlots;
            private Command UndoCommand = new NoCommand();

            public SimpleRemoteControl()
            {
                OnSlots = new Command[2];
                OffSlots = new Command[2];

                for(int i = 0; i < OnSlots.Length; i++)
                {
                    OnSlots[i] = new NoCommand();
                }

                for (int i = 0; i < OffSlots.Length; i++)
                {
                    OffSlots[i] = new NoCommand();
                }
            }

            public void SetOnSlot(int index, Command slot)
            {
                if(index < this.OnSlots.Length &&  slot != null)
                {
                    this.OnSlots[index] = slot;
                }                
            }

            public void SetOffSlot(int index, Command slot)
            {
                if (index < this.OnSlots.Length && slot != null)
                {
                    this.OffSlots[index] = slot;
                }
            }

            public void OnButtonWasPressed(int index)
            {                
                if(index < this.OnSlots.Length)
                {
                    this.OnSlots[index].Execute();
                    this.UndoCommand = this.OnSlots[index];
                }                
            }

            public void OffButtonWasPressed(int index)
            {
                if (index < this.OffSlots.Length)
                {
                    this.OffSlots[index].Execute();
                    this.UndoCommand = this.OffSlots[index];
                }
            }

            public void UndoButtonWasPushed()
            {
                this.UndoCommand.Undo();
            }
        }
    }
}
