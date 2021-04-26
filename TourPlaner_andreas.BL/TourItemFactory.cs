namespace TourPlaner_andreas.BL {
    public static class TourItemFactory {
        private static ITourItem_Manager manager;

        public static ITourItem_Manager GetFactoryManager() {
            if (manager == null) {
                manager = new TourItemFactoryImpl();
            }
            return manager;
        }
    }
}
