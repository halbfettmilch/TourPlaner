namespace TourPlaner_andreas.BL {
    public static class WpfAppManagerFactory {
        private static IWpfAppManager manager;

        public static IWpfAppManager GetFactoryManager() {
            if (manager == null) {
                manager = new WpfAppManagerImpl();
            }
            return manager;
        }
    }
}
