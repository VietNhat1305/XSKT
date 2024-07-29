
const toJson = (item , listMenu) => {
    return {
        _id: item._id,
        name: item.name,
        noiDung: item.noiDung,
        files: item.files,
        menu: toListModel(item.menu),
        chuyenMuc: item.chuyenMuc,
        soKiHieu: item.soKiHieu,
        ngayKy: item.ngayKy,
        fileImage:item.fileImage,
        isPublic:item.isPublic,
        hoVaTen:item.hoVaTen,
        soDienThoai: item.soDienThoai,
        ngayXuatBan:item.ngayXuatBan,
        trichYeu:item.trichYeu,
        moTa:item.moTa
    }
}
const fromJson = (item) => {
    return {
        _id: item._id,
        name: item.name,
        noiDung: item.noiDung,
        files: item.files,
        menu: item.menu,
        chuyenMuc: item.chuyenMuc,
        soKiHieu: item.soKiHieu,
        ngayKy: item.ngayKy? new Date(item.ngayKy):item.ngayKy,
        fileImage:item.fileImage,
        isPublic:item.isPublic,
        hoVaTen:item.hoVaTen,
        soDienThoai: item.soDienThoai,
        ngayXuatBan: item.ngayXuatBan? new Date(item.ngayXuatBan):item.ngayXuatBan,
        trichYeu:item.trichYeu,
        moTa:item.moTa
    }
}

const baseJson = () => {
    return {
        _id: null,
        name: null,
        noiDung: null,
        menu: [],
        files:[],
        soKiHieu: null,
        ngayKy: null,
        fileImage:null,
        isPublic:true,
        hoVaTen:null,
        soDienThoai: null,
        ngayXuatBan:null,
        trichYeu:null,
        moTa:null
    }
}

const toListModel = (items) =>{
    if(items.length > 0){
        let data = [];
        items.map((value, index) =>{
            data.push({id : value.id , name : value.label != null ? value.label : value.name});
        })
        return data??[];
    }
    return [];
}

export const  contentNoiBoModel = {
    toJson, fromJson, baseJson, toListModel
}
