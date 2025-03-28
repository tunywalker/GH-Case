document.addEventListener("DOMContentLoaded", function () {
    const burgerBtn = document.querySelector(".header-burger-menu");
    const mobileMenu = document.querySelector(".header__mobile-menu");

    if (burgerBtn && mobileMenu) {
        burgerBtn.addEventListener("click", () => {
            const isOpen = mobileMenu.classList.contains("active");

            if (!isOpen) {
                mobileMenu.classList.remove("d-none");
                setTimeout(() => {
                    mobileMenu.classList.add("active");
                }, 10);
            } else {
                mobileMenu.classList.remove("active");
                setTimeout(() => {
                    mobileMenu.classList.add("d-none");
                }, 400);
            }

            burgerBtn.classList.toggle("active");
        });
    }
});
document.querySelectorAll('.header__mobile-menu-title').forEach(title => {
    title.addEventListener('click', () => {
        title.classList.toggle('active');

        const submenu = title.nextElementSibling;
        if (submenu && submenu.classList.contains('header__mobile-menu-sub-list')) {
            submenu.classList.toggle('active');
        }
    });
});
