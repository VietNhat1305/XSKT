<script>
import Layout from "@/layouts/main";
import PageHeader from "@/components/page-header";
import {required} from "vuelidate/lib/validators";
import appConfig from "@/app.config";
import {pagingModel} from "@/models/pagingModel";
import vue2Dropzone from "vue2-dropzone";
import axios from "axios";
import {thuvienanhModel} from "@/models/thuvienanhModel";
import Multiselect from "vue-multiselect";
export default {
  page: {
    title: "Quản lý thư viện ảnh",
    meta: [{name: "description", content: appConfig.description}],
  },
  components: {Layout, PageHeader, vueDropzone: vue2Dropzone, },
  data() {
    return {
      title: "Quản lý thư viện ảnh",
      items: [
        {
          text: "Thư viện ảnh",
          href: "/thu-vien-anh",
          // active: true,
        },
        {
          text: "Danh sách",
          active: true,
        }
      ],
      data: [],
      showModal: false,
      showModalSlider: false,
      showDetail: false,
      showDeleteModal: false,
      submitted: false,
      modelSlider: thuvienanhModel.baseJson(),
      listRole: [],
      pagination: pagingModel.baseJson(),
      totalRows: 1,
      todoTotalRows: 1,
      currentPage: 1,
      numberOfElement: 1,
      perPage: 10,
      pageOptions: [5, 10, 25, 50, 100],
      filter: null,
      todoFilter: null,
      filterOn: [],
      todofilterOn: [],
      sortBy: "age",
      sortDesc: false,
      images: null,
      fields: [
        {
          key: 'STT', label: 'STT',
          class: "text-center td-stt",
          tdClass: 'align-middle',
          thClass: 'hidden-sortable'
        },
        {
          key: "name",
          label: "Mô tả",
          class: "text-center",
          thClass: 'hidden-sortable'
        },
        {
          key: "file",
          label: "Hình ảnh",
          class: "text-center",
          thClass: 'hidden-sortable'
        },
        {
          key: 'process',
          label: 'Xử lý',
          class: "text-center",
          thStyle: {width: '100px', minWidth: '60px'},
          thClass: 'hidden-sortable'
        }
      ],
      listFileSilder : [],
      url : `${process.env.VUE_APP_API_URL}filesminio/view/`,
      dropzoneOptions: {
        url: `${process.env.VUE_APP_API_URL}filesminio/upload`,
        acceptedFiles: ".jpg,.jpeg,.png,.gif",
        thumbnailHeight: 100,
        thumbnailWidth: 300,
        maxFiles: 1,
        maxFilesize: 30,
        headers: {"My-Awesome-Header": "header value"},
        addRemoveLinks: true
      },
      image: "",
      file: "",
      urlFile:`${process.env.VUE_APP_API_URL}filesminio/view`
    };
  },
  validations: {
    modelSlider: {
      name: {required},
    },
  },
  created() {
    // this.getThongTinSlider();
    const user = {
      id: 1,
      name: 'Victory Osayi',
      is_editor: true,
      is_admin: false,
      // you can have role based permission list or access control list possibly from database
      permissions: [
        'manage-THUVIENANH-655da6ca5d377463faffe794', 
        'create-THUVIENANH-655da6ca5d377463faffe794',
        'update-THUVIENANH-655da6ca5d377463faffe794',
        'delete-THUVIENANH-655da6ca5d377463faffe794'
      ]
    }
  },
  watch: {
    showModalSlider(status) {
      if (status == false) this.modelSlider = thuvienanhModel.baseJson() ;
    },
    showDeleteModal(val) {
      if (val == false) {
        this.model.id = null;
      }
    },
  },
  methods: {
    uploadFileSlider(files,response) {
      let fileSuccess = response.data;
      console.log('log suscess', response.data)
      if (response.code == 0){
        this.modelSlider.file={
          fileId: fileSuccess.fileId,
          fileName: fileSuccess.fileName,
          ext: fileSuccess.ext,
          path: fileSuccess.path
        };
      }
      console.log('log model file', this.model.file)

    },
    removeThisFile(files, error, xhr) {
      let fileCongViec = JSON.parse(files.xhr.response);
      if (fileCongViec.data && fileCongViec.data.fileId) {
        let idFile = fileCongViec.data.fileId;
        let resultData = this.model.file.filter(x => {
          return x.fileId != idFile;
        })
        this.model.file = resultData;
      }
    },
    // sendingEvent(file, xhr, formData) {
    //   formData.append("files", file);
    // },

    // async getThongTinSlider(){
    //   let promise =  this.$store.dispatch("thuvienanhStore/getAll")
    //   return promise.then(resp => {
    //     if(resp.data == null){
    //       return []
    //     }else{
    //       if (resp.data != null )
    //       {
    //         this.listFileSilder = []
    //         this.listFileSilder = resp.data;
    //       }
    //     }
    //   })
    // },

    async handleSubmitSlider(e) {
      e.preventDefault();
      this.submitted = true;
      this.$v.$touch();
      if (this.$v.$invalid) {
        return;
      } else {
        let loader = this.$loading.show({
          container: this.$refs.formContainer,
        });
        if (
            this.modelSlider._id != 0 &&
            this.modelSlider._id != null &&
            this.modelSlider._id
        ) {
          console.log("LOG USER UPDATE NE : " , this.modelSlider)
          if (this.modelSlider == null || (this.modelSlider != null && this.modelSlider.file == null)) {
            this.$store.dispatch("snackBarStore/addNotify", {
              message: "Vui lòng chọn hình ảnh trước khi lưu ",
              code: 20,
            });
            loader.hide();
            return;
          }else{
            this.modelSlider.loaiAnhId=this.$route.params.id
            await this.$store.dispatch("thuvienanhStore/update", this.modelSlider).then((res) => {
              if (res.code === 0) {
                this.showModalSlider = false;
                this.modelSlider=thuvienanhModel.baseJson()
                this.$refs.tblList.refresh();
              }
              this.$store.dispatch("snackBarStore/addNotify", {
                message: res.message,
                code: res.code,
              });
            });
          }
          // Update model

        } else {
          // Create model
          console.log("LOG USER CREATE NE : " , this.modelSlider)
          if (this.modelSlider == null || (this.modelSlider != null && this.modelSlider.file == null)) {
            this.$store.dispatch("snackBarStore/addNotify", {
              message: "Vui lòng chọn hình ảnh trước khi lưu ",
              code: 20,
            });
            loader.hide();
            return;
          }else{
            this.modelSlider.loaiAnhId=this.$route.params.id
            await this.$store.dispatch("thuvienanhStore/create", this.modelSlider).then((res) => {
              if (res.code === 0) {
                this.fnGetList();
                this.showModalSlider = false;
                this.model = {}
              }
              this.$store.dispatch("snackBarStore/addNotify", {
                message: res.message,
                code: res.code,
              });
            });
          }

        }
        loader.hide();
      }
      this.submitted = false;
    },
    handleShowDeleteModal(id) {
      this.modelSlider._id = id;
      this.showDeleteModal = true;
    },
    async handleDelete() {
      if (this.modelSlider._id != 0 && this.modelSlider._id != null && this.modelSlider._id) {
        await this.$store.dispatch("thuvienanhStore/delete",{_id :  this.modelSlider._id}).then((res) => {
          if (res.code===0) {
            this.fnGetList();
            this.modelSlider=thuvienanhModel.baseJson()
            this.showDeleteModal = false;
          }
          var a = {
            message: res.message,
            code: res.code
          };
          // console.log("LOG A : " ,a)
          this.$store.dispatch("snackBarStore/addNotify", {
            message: res.message,
            code: res.code
          });
        });
      }
    },
    async handleUpdate(id) {
      await this.$store.dispatch("thuvienanhStore/getById", {_id : id}).then((res) => {
        if (res.code===0) {
          console.log(res)
          this.modelSlider = thuvienanhModel.toJson(res.data);
          this.showModalSlider = true;
        } else {
          this.$store.dispatch("snackBarStore/addNotify", {
            message: res.message,
            code: res.code,
          });
        }
      });
    },

    myProvider (ctx) {
      const params = {
        start: ctx.currentPage,
        limit: ctx.perPage,
        content: this.filter,
        sortBy: 'thuTu',
        loaiAnhId : this.$route.params.id
      }
      this.loading = true
      try {
        let promise =  this.$store.dispatch("thuvienanhStore/getPagingParams", params)
        return promise.then(resp => {
          let items = resp.data.data
          this.totalRows = resp.data.totalRows
          this.numberOfElement = resp.data.data.length
          this.loading = false
          return items || []
        })
      } finally {
        this.loading = false
      }
    },
    async fnGetList() {
      this.$refs.tblList?.refresh()
    },
    getColorWithExtFile(ext) {
      if (ext == '.png' || ext == '.jpg'|| ext == '.jpeg' )
        return 'text-danger';

    },

    getIconWithExtFile(ext) {
      if (ext == '.png' || ext == '.jpg'|| ext == '.jpeg')
        return 'mdi mdi-file-image-outline';
    },
    deleteFile(id) {
      let resultData = this.modelSlider.file.filter(x => {
        return x.fileId !=id;
      })
      this.modelSlider.file = resultData;
      console.log('log model file remove', this.modelSlider.files)
      axios.post(`${process.env.VUE_APP_API_URL}filesminio/delete/`+id).then((response) => {
        console.log('log model file remove', this.modelSlider.files);
      }).catch((error) => {
        // Handle error here
        console.error('Error deleting file:', error);
      });
    },

  }
}
</script>
<template>
  <Layout>
    <PageHeader :title="title" :items="items"/>
    <div class="row">
      <div class="col-12">
        <div class="card">
          <div class="card-body">
            <div class="row mb-2">
              <div class="col-sm-4">
                <div class="search-box me-2 mb-2 d-inline-block">
                  <div class="position-relative">
                    <input
                        v-model="filter"
                        type="text"
                        class="form-control"
                        placeholder="Tìm kiếm ..."
                    />
                    <i class="bx bx-search-alt search-icon"></i>
                  </div>
                </div>
              </div>
              <div class="col-sm-8">
                <div class="text-sm-end">
                  <b-button
                      v-if="$can('manage-THUVIENANH-655da6ca5d377463faffe794') || $can('create-THUVIENANH-655da6ca5d377463faffe794')"
                      type="button"
                      class="btn cs-btn-primary btn-rounded mb-2 me-2"
                      size="sm"
                      @click="showModalSlider = true"
                  >
                    <i class="mdi mdi-plus me-1"></i> Thêm ảnh
                  </b-button>
                  <b-modal
                      v-model = "showModalSlider"
                      title="Thêm ảnh"
                      title-class="text-black font-18"
                      body-class="p-3"
                      hide-footer
                      centered
                      no-close-on-backdrop
                      size="lg"
                  >
                    <form @submit.prevent="handleSubmitSlider"
                          ref="formContainer"
                    >
                      <div class="row">
                        <div class="col-6">
                          <div class="mb-2">
                            <label class="text-left mb-0">Tên ảnh</label>
                            <span style="color: red">&nbsp;*</span>
                            <input
                                v-model="modelSlider.name"
                                id="percent"
                                type="link"
                                class="form-control"
                                :class="{
                                  'is-invalid':
                                    submitted && $v.modelSlider.name.$error,
                                }"
                            />
                            <div
                                v-if="submitted && !$v.modelSlider.name.required"
                                class="invalid-feedback"
                            >
                              Mô tả không được trống.
                            </div>
                          </div>
                        </div>
                        
                        <div class="col-12" >
                          <label class="text-left">Chọn ảnh (Kích thước 350 x 150) </label>
                          <!-- <template  v-if="modelSlider.file.length > 0" >
                            <div v-for="(file,index) in modelSlider.file" :key="index">
                              <a
                                  class="ml-25"
                                  :href="`${urlFile}/${file.fileId}`"
                              ><i
                                  :class="`${getColorWithExtFile(file.ext)} me-2 ${getIconWithExtFile(file.ext)}`"
                              ></i>{{ file.fileName }}</a>
                              <b-button
                                  variant="flat-danger"
                                  class="btn-icon"
                                  @click="deleteFile(file.fileId)"
                              >
                                <i class="mdi mdi-trash-can-outline"></i>
                              </b-button>
                            </div>
                          </template> -->
                          <template  v-if="modelSlider.file" >
                            <a
                                class="ml-25"
                                :href="`${urlFile}/${modelSlider.file.fileId}`"
                            ><i
                                :class="`${getColorWithExtFile(modelSlider.file.ext)} me-2 ${getIconWithExtFile(modelSlider.file.ext)}`"
                            ></i>{{modelSlider.file.fileName }}</a>
                              <b-button
                                  variant="flat-danger"
                                  class="btn-icon"
                                  @click="deleteFile()"
                              >
                                <i class="mdi mdi-trash-can-outline"></i>
                              </b-button>

                          </template>

                          <vue-dropzone
                              id="dropzone"
                              ref="myVueDropzone"
                              :use-custom-slot="true"
                              :options="dropzoneOptions"

                              v-on:vdropzone-removed-file="removeThisFile"
                              v-on:vdropzone-success="uploadFileSlider"
                          >
                            <div class="dropzone-custom-content">
                              <div class="mb-1">
                                <i class="display-4 text-muted bx bxs-cloud-upload"></i>
                              </div>
                              <h4>Kéo thả tệp hoặc click vào đây để tải tệp tin.</h4>
                            </div>
                          </vue-dropzone>
                        </div>
                        <div class="col-12">
                          <div class="mb-2">
                            <label class="text-left mb-0">Thứ tự</label>
                            <input
                                id="sort"
                                v-model="modelSlider.thuTu"
                                type="number"
                                class="form-control"
                                placeholder="Nhập thứ tự"
                            />
                          </div>
                        </div>

                      </div>
                      <div class="text-end pt-2 mt-3">
                        <b-button variant="light" @click="showModalSlider = false" class="border-0">
                          Đóng
                        </b-button>
                        <b-button  type="submit" variant="success" class="ms-1 cs-btn-primary">Lưu
                        </b-button>
                      </div>
                    </form>
                  </b-modal>
                </div>
              </div>
            </div>
            <div class="row">
              <div class="col-12">
                <div class="row mt-4">
                  <div class="col-sm-12 col-md-6">
                    <div
                        class="col-sm-12 d-flex justify-content-left align-items-center"
                    >
                      <div
                          id="tickets-table_length"
                          class="dataTables_length m-1"
                          style="
                        display: flex;
                        justify-content: left;
                        align-items: center;
                      "
                      >
                        <div class="me-1" >Hiển thị </div>
                        <b-form-select
                            class="form-select form-select-sm"
                            v-model="perPage"
                            size="sm"
                            :options="pageOptions"
                            style="width: 70px"
                        ></b-form-select
                        >&nbsp;
                        <div style="width: 100px"> dòng </div>
                      </div>
                    </div>
                  </div>
                </div>
                <div class="table-responsive mb-0">
                  <b-table
                      class="datatables table-admin"
                      :items="myProvider"
                      :fields="fields"
                      striped
                      bordered
                      responsive="sm"
                      :per-page="perPage"
                      :current-page="currentPage"
                      :sort-by.sync="sortBy"
                      :sort-desc.sync="sortDesc"
                      :filter="filter"
                      :filter-included-fields="filterOn"
                      ref="tblList"
                      primary-key="id"
                  >
                    <template v-slot:cell(STT)="data">
                      {{ data.index + ((currentPage-1)*perPage) + 1  }}
                    </template>
                    <template v-slot:cell(file)="data">
                        <b-card-img
                          :src="url + data.item.file.fileId"
                          class="rounded-0 image-pakn mb-2"
                          :height="300"
                          :width="300"
                      ></b-card-img>

                    </template>
                    
                    <template v-slot:cell(process)="data">
                      <button
                          v-if="$can('manage-THUVIENANH-655da6ca5d377463faffe794') || $can('update-THUVIENANH-655da6ca5d377463faffe794')"
                          type="button"
                          size="sm"
                          class="btn btn-outline btn-sm"
                          v-on:click="handleUpdate(data.item._id)">
                        <i class="fas fa-pencil-alt text-success me-1"></i>
                      </button>
                      <button
                          v-if="$can('manage-THUVIENANH-655da6ca5d377463faffe794') || $can('delete-THUVIENANH-655da6ca5d377463faffe794')"
                          type="button"
                          size="sm"
                          class="btn btn-outline btn-sm"
                          v-on:click="handleShowDeleteModal(data.item._id)">
                        <i class="fas fa-trash-alt text-danger me-1"></i>
                      </button>
                    </template>
                  </b-table>
                </div>
                <div class="row">
                  <b-col>
                    <div>Hiển thị {{numberOfElement}} trên tổng số {{totalRows}} dòng</div>
                  </b-col>
                  <div class="col">
                    <div
                        class="dataTables_paginate paging_simple_numbers float-end">
                      <ul class="pagination pagination-rounded mb-0">
                        <!-- pagination -->
                        <b-pagination
                            v-model="currentPage"
                            :total-rows="totalRows"
                            :per-page="perPage"
                        ></b-pagination>
                      </ul>
                    </div>
                  </div>
                </div>

              </div>
            </div>
            <b-modal
                v-model="showDeleteModal"
                centered
                title="Xóa dữ liệu"
                title-class="font-18"
                no-close-on-backdrop
            >
              <p>
                Dữ liệu xóa sẽ không được phục hồi!
              </p>
              <template #modal-footer>
                <button v-b-modal.modal-close_visit
                        class="btn btn-outline-info m-1"
                        v-on:click="showDeleteModal = false">
                  Đóng
                </button>
                <button v-b-modal.modal-close_visit
                        class="btn btn-danger btn m-1"
                        v-on:click="handleDelete">
                  Xóa
                </button>
              </template>
            </b-modal>
          </div>
        </div>
      </div>
    </div>
  </Layout>
</template>
