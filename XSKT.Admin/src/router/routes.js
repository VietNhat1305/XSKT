import store from "@/state/store";
export default [
  // {
  //     path: "/dang-nhap",
  //     name: "login",
  //     component: () => import("../pages/quantri/login/login"),
  //     meta: {
  //         beforeResolve(routeTo, routeFrom, next) {
  //             if (store.getters["authStore/loggedIn"]) {
  //                 next({name: "default"});
  //             } else {
  //                 next();
  //             }
  //         }
  //     },
  // },
  {
      path: "/404",
      name: "login",
      component: () => import("../pages/utility/404"),
      meta: {
          beforeResolve(routeTo, routeFrom, next) {
              if (store.getters["authStore/loggedIn"]) {
                  next({name: "default"});
              } else {
                  next();
              }
          }
      },
  },
  {
    path: "/dang-nhap",
    name: "default",
    meta: {},
    component: () => import("../pages/quantri/login/login"),
  },
  {
    path: "/trang-chu-1",
    name: "default",
    meta: {},
    component: () => import("../pages/congdan/trangchu1"),
  },
  {
    path: "/trang-chu-2",
    name: "default",
    meta: {},
    component: () => import("../pages/congdan/trangChu2.vue"),
  },
  {
    path: "/trang-chu-3",
    name: "default",
    meta: {},
    component: () => import("../pages/congdan/trangchu3"),
  },
  {
    path: "/trang-chu-4",
    name: "default",
    meta: {},
    component: () => import("../pages/congdan/trangchu4"),
  },
  {
    path: "/",
    name: "default",
    meta: {},
    component: () => import("../pages/congdan/trangchu1"),
  },
  {
    path: "/gioi-thieu/:id?",
    name: "Giới thiệu công ty",
    meta: {},
    component: () => import("../pages/category"),
  },
  {
    path: "/tin-hoat-dong/:id?",
    name: "Hoạt động công ty",
    meta: {},
    component: () => import("../pages/category"),
  },
  {
    path: "/diem-tin-hang-tuan/:id?",
    name: "Hoạt động công ty",
    meta: {},
    component: () => import("../pages/category"),
  },
  {
    path: "/du-lich-va-dac-san-vung-mien/:id?",
    name: "Hoạt động công ty",
    meta: {},
    component: () => import("../pages/category"),
  },

  {
    path: "/bang-tin/chi-tiet/:id?",
    name: "Hoạt động",
    meta: {},
    component: () => import("../pages/category/chitiet"),
  },
  {
    path: "/dau-gia/chi-tiet/:id?",
    name: "Đấu giá đấu thầu",
    meta: {},
    component: () => import("../pages/category/chitiet"),
  },
  {
    path: "/lien-he/:id?",
    name: "Liên hệ",
    meta: {},
    component: () => import("../pages/lienhe"),
  },
  {
    path: "/cong-bo-thong-tin/:id?",
    name: "Vai trò",
    meta: {},
    component: () => import("../pages/category"),
  },
  {
    path: "/van-ban-phap-quy/:id?",
    name: "Văn bản pháp quy",
    meta: {},
    component: () => import("../pages/vanbanphapquy"),
  },
  {
    path: "/dau-gia/:id?",
    name: "Văn bản pháp quy",
    meta: {},
    component: () => import("../pages/vanbanphapquy"),
  },
  {
    path: "/dau-thau/:id?",
    name: "Văn bản pháp quy",
    meta: {},
    component: () => import("../pages/dauthau"),
  },
  {
    path: "/xem-chi-tiet/:id?",
    name: "Xem chi tiết",
    meta: {},
    component: () => import("../pages/ShowPDF"),
  },
  {
    path: "/trung-thuong/:id?",
    name: "Trúng thưởng",
    meta: {},
    component: () => import("../pages/category"),
  },
  // {
  //     path: "/xem-tep-tin/:id?",
  //     name: "Xem tệp tin",
  //     component: () => import("../pages/ShowPDF"),
  // },
  {
    path: "/danh-muc-tin/:id?",
    name: "category",
    meta: {},
    component: () => import("../pages/category"),
  },
  {
    path: "/hoi-dap",
    name: "Hỏi đáp",
    meta: {},
    component: () => import("../pages/hoidap"),
  },
  {
    path: "/hoi-dap/chi-tiet/:id?",
    name: "Hoạt động",
    meta: {},
    component: () => import("../pages/category/chitiet"),
  },
  {
    path: "/bien-ban/chi-tiet/:id?",
    name: "Biên bản",
    meta: {},
    component: () => import("../pages/ShowPDF"),
  },

  // QUAN TRI

  {
    path: "/menu",
    name: "menu",
    meta: {},
    component: () => import("../pages/quantri/menu"),
  },

  {
    path: "/xo-so",
    name: "Kết quả xổ số",
    meta: {},
    component: () => import("../pages/quantri/ketQuaXoSo"),
  },

  {
    path: "/tai-khoan",
    name: "Tài khoản",
    meta: {},
    component: () => import("../pages/quantri/user"),
  },
  {
    path: "/tai-khoan/doi-mat-khau",
    name: "Đổi mật khẩu",
    meta: {},
    component: () => import("../pages/quantri/user/ChangePass"),
  },
  {
    path: "/menu-cong-dan",
    name: "menu",
    meta: {},
    component: () => import("../pages/quantri/menucongdan"),
  },
  {
    path: "/vai-tro",
    name: "Quyền",
    meta: {},
    component: () => import("../pages/quantri/unitRole"),
  },
  {
    path: "/vai-tro/thiet-lap-quyen/:id?",
    name: "Vai trò",
    meta: {},
    component: () => import("../pages/quantri/unitRole/addPermissions"),
  },

  {
    path: "/nhom-quyen",
    name: "Quản lý nhóm quyền",
    meta: {},
    component: () => import("../pages/quantri/module"),
  },
  {
    path: "/nhom-quyen/action/:id?",
    name: "Hành động",
    // meta: {},
    component: () => import("../pages/quantri/module/action"),
  },
  {
    path: "/thong-tin-ca-nhan",
    name: "Thông tin cá nhân",
    // meta: {},
    component: () => import("../pages/quantri/login/profile"),
  },
  {
    path: "/tao-bai-viet",
    name: "Tạo bài viết",
    // meta: {},
    component: () => import("../pages/quantri/taobaiviet"),
  },
  {
    path: "/danh-sach",
    name: "Danh sách bài viết",
    // meta: {},
    component: () => import("../pages/quantri/danhsachbaiviet"),
  },
  {
    path: "/thong-tin-lien-he",
    name: "Quản lý thông tin liên hệ",
    meta: {},
    component: () => import("../pages/quantri/lienhe"),
  },
  {
    path: "/Slider",
    name: "Quản lý Slider",
    meta: {},
    component: () => import("../pages/quantri/slider"),
  },
  {
    path: "/danh-sach/chi-tiet/:id?",
    name: "Chi tiết bài viết",
    // meta: {},
    component: () =>
      import("../pages/quantri/danhsachbaiviet/capNhatBaiViet.vue"),
  },
  {
    path: "/su-kien",
    name: "Quản lý sự kiện",
    // meta: {},
    component: () => import("../pages/quantri/suKien"),
  },

  {
    path: "/404",
    name: "404",
    component: require("../pages/utility/404").default,
  },
  {
    path: "/unauthorized",
    name: "401",
    component: require("../pages/utility/401").default,
  },
  {
    path: "*",
    redirect: "404",
  },
  {
    path: "/lien-ket",
    name: "Banner liên kết",
    // meta: {},
    component: () => import("../pages/quantri/link"),
  },
  {
    path: "/thu-vien-anh",
    name: "Quản lý thư viện ảnh",
    // meta: {},
    component: () => import("../pages/quantri/thuvienanh"),
  },
  {
    path: "/loai-anh/chi-tiet/:id?",
    name: "Thư viện ảnh",
    meta: {},
    component: () => import("../pages/chitietanh"),
  },
  {
    path: "/loai-anh",
    name: "Quản lý loại ảnh",
    // meta: {},
    component: () => import("../pages/quantri/loaiAnh"),
  },
  {
    path: "/loai-anh/thu-vien-anh/:id?",
    name: "Thiết lập ảnh",
    meta: {},
    component: () => import("../pages/quantri/loaiAnh/addPermissions"),
  },
  {
    path: "/quan-ly-video",
    name: "Quản lý video",
    // meta: {},
    component: () => import("../pages/quantri/linkYoutube"),
  },
  {
    path: "/quan-ly-website",
    name: "Quản lý liên kết",
    // meta: {},
    component: () => import("../pages/quantri/lienKetWebsite"),
  },
  {
    path: "/quan-ly-hinh-anh",
    name: "Quản lý hình ảnh",
    // meta: {},
    component: () => import("../pages/quantri/hinhAnh"),
  },
  {
    path: "/quan-ly-diem-ban",
    name: "Quản lý điểm bán",
    // meta: {},
    component: () => import("../pages/quantri/diemBanSoLoTo"),
  },
  {
    path: "/diem-ban-lo-to",
    name: "Quản lý điểm bán vé số lô tô",
    // meta: {},
    component: () => import("../pages/quantri/diemBanVeLoTo"),
  },
  {
    path: "/quan-ly-bien-ban",
    name: "Quản lý biên bản",
    // meta: {},
    component: () => import("../pages/quantri/bienBanXoSo"),
  },
  {
    path: "/danh-sach-hoi-dap",
    name: "Danh sách hỏi đáp",
    // meta: {},
    component: () => import("../pages/quantri/hoidap"),
  },
  {
    path: "/tao-tin-noi-bo",
    name: "Tin nội bộ",
    // meta: {},
    component: () => import("../pages/quantri/tinnoibo"),
  },
  {
    path: "/tin-noi-bo/:id?",
    name: "Tin nội bộ",
    // meta: {
    //           beforeResolve(routeTo, routeFrom, next) {
    //               if (store.getters["authStore/loggedIn"]) {
    //                   next({name: "default"});
    //               } else {
    //                   next();
    //               }
    //           }
    //       },
    // meta: {},
    component: () => import("../pages/quantri/danhsachtinnoibo"),
  },
];
