namespace Xenon {
	public interface IEventSender {
		
	}

	public static class IEventSenderExt {

		public static void Send(this IEventSender sender, XEvent ev) {
			EventManager.I.SendEvent(sender, ev);
		}

	}
}
