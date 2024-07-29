const toJson = (item) => {
    return {
        _id: item._id,
        name: item.name,
        diaChi: item.diaChi,
        soDienThoai: item.soDienThoai,
        fax: item.fax,
        email: item.email,
        vanPhongDaiDien: item.vanPhongDaiDien,
        maSoThue: item.maSoThue,
        nguoiBienTap: item.nguoiBienTap
    }
}

const fromJson = (item) => {
    return {
        _id: item._id,
        name: item.name,
        diaChi: item.diaChi,
        soDienThoai: item.soDienThoai,
        fax: item.fax,
        email: item.email,
        vanPhongDaiDien: item.vanPhongDaiDien,
        maSoThue: item.maSoThue,
        nguoiBienTap: item.nguoiBienTap
    }
}

const baseJson = () => {
    return {
        _id: null,
        name: null,
        diaChi:null,
        soDienThoai: null,
        fax: null,
        email:null,
        vanPhongDaiDien: null,
        maSoThue: null,
        nguoiBienTap: null
    }
}

const toListModel = (items) =>{
    if(items.length > 0){
        let data = [];
        items.map((value, index) =>{
            data.push(fromJson(value));
        })
        return data??[];
    }
    return [];
}
export const lienHeModel = {
    toJson, fromJson, baseJson, toListModel
}
