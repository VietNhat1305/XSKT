
const toJson = (item) => {
    return {
        _id: item._id,
        name: item.name,
        hoVaTen : item.hoVaTen,
        menu : item.menu,
        soDienThoai: item.soDienThoai,
        email: item.email,
        isPublic: item.isPublic,
        diaChi: item.diaChi,
        files: item.files,
    }
}
const fromJson = (item) => {
    return {
        _id: item._id,
        name: item.name,
        hoVaTen : item.hoVaTen,
        menu : item.menu,
        soDienThoai: item.soDienThoai,
        email: item.email,
        isPublic: item.isPublic,
        diaChi: item.diaChi,
        files: item.files,
    }
}

const baseJson = () => {
    return {
        _id: null,
        name: null,
        hoVaTen : null,
        menu : [],
        soDienThoai: null,
        email: null,
        isPublic: false,
        diaChi: null,
        files: [],
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

export const  hoiDapModel = {
    toJson, fromJson, baseJson, toListModel
}
