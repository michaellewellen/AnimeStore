window.cartCookie = {
    get: function () {
        const match = document.cookie.match(/(?:^|; )guestToken=([^;]*)/);
        return match ? match[1] : null;
    },
    set: function (value) {
        document.cookie = "guestToken=" + value + "; path=/; max-age=" + (60 * 60 * 24 * 30);
    }
};