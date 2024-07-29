const toJson = (item) => {
  return {
    _id: item._id,
    name: item.name,
    path: item.path,
    icon: item.icon,
    parentId: item.parentId,
    level: item.level,
    sort: item.sort,
    isShow: item.isShow,
    listAction: item.listAction,
    isPrivate: item.isPrivate,
    isTieuDe: item.isTieuDe,
    isMoTa: item.isMoTa,
    isTrichYeu: item.isTrichYeu,
    isKyHieu: item.isKyHieu,
    isNgayXuatBan: item.isNgayXuatBan,
    isAnhDaiDien: item.isAnhDaiDien,
    isNgayKy: item.isNgayKy,
    isNoiDung: item.isNoiDung,
    isTepTin: item.isTepTin,
    // show menu
    isShowTieuDe: item.isShowTieuDe,
    isShowMoTa: item.isShowMoTa,
    isShowTrichYeu: item.isShowTrichYeu,
    isShowKyHieu: item.isShowKyHieu,
    isShowNgayXuatBan: item.isShowNgayXuatBan,
    isShowAnhDaiDien: item.isShowAnhDaiDien,
    isShowNgayKy: item.isShowNgayKy,
    isShowNoiDung: item.isShowNoiDung,
    isShowTepTin:item.isShowTepTin,
    isTrangChu: item.isTrangChu,
    soLuongTin: item.soLuongTin,
    isMenu: item.isMenu,
    isDanhMuc: item.isDanhMuc,
  };
};

const fromJson = (item) => {
  return {
    _id: item._id,
    name: item.name,
    path: item.path,
    icon: item.icon,
    parentId: item.parentId,
    level: item.level,
    isShow: item.isShow,
    sort: item.sort,
    listAction: item.listAction,
    label: item.label,
    subItems: item.subItems || [],
    isPrivate: item.isPrivate,
    isTieuDe: item.isTieuDe,
    isMoTa: item.isMoTa,
    isTrichYeu: item.isTrichYeu,
    isKyHieu: item.isKyHieu,
    isNgayXuatBan: item.isNgayXuatBan,
    isAnhDaiDien: item.isAnhDaiDien,
    isNgayKy: item.isNgayKy,
    isNoiDung: item.isNoiDung,
    isTepTin: item.isTepTin,
    // show menu
    isShowTieuDe: item.isShowTieuDe,
    isShowMoTa: item.isShowMoTa,
    isShowTrichYeu: item.isShowTrichYeu,
    isShowKyHieu: item.isShowKyHieu,
    isShowNgayXuatBan: item.isShowNgayXuatBan,
    isShowAnhDaiDien: item.isShowAnhDaiDien,
    isShowNgayKy: item.isShowNgayKy,
    isShowNoiDung: item.isShowNoiDung,
    isTrangChu: item.isTrangChu,
    soLuongTin: item.soLuongTin,
    isShowTepTin:item.isShowTepTin,
    isMenu: item.isMenu,
    isDanhMuc: item.isDanhMuc,
  };
};

const baseJson = () => {
  return {
    _id: null,
    name: null,
    path: null,
    icon: null,
    parentId: null,
    listAction: null,
    level: 0,
    sort: 0,
    isShow: null,
    isPrivate: false,
    isTieuDe: false,
    isMoTa: false,
    isTrichYeu: false,
    isKyHieu: false,
    isNgayXuatBan: false,
    isAnhDaiDien: false,
    isNgayKy: false,
    isNoiDung: false,
    isTepTin: false,
    //show menu
    isShowTieuDe: false,
    isShowMoTa: false,
    isShowTrichYeu: false,
    isShowKyHieu: false,
    isShowNgayXuatBan: false,
    isShowAnhDaiDien: false,
    isShowNgayKy: false,
    isShowNoiDung: false,
    isShowTepTin: false,

    isTrangChu: false,
    isMenu: false,
    isDanhMuc: false,
  };
};

const toListModel = (items) => {
  if (items.length > 0) {
    let data = [];
    items.map((value, index) => {
      data.push(fromJson(value));
    });
    return data ?? [];
  }
  return [];
};

export const menuModel = {
  toJson,
  fromJson,
  baseJson,
  toListModel,
};
